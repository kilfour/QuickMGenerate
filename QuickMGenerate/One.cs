using System.Reflection;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T> One<T>()
		{
			return
				s =>
					{
						var instance = (T)CreateInstance(s, typeof(T));
						BuildInstance(instance, s, new Stack<Type>(), typeof(T));
						return new Result<T>(instance, s);
					};
		}

		public static Generator<T> One<T>(Func<T> constructor)
		{
			return
				s =>
				{
					var instance = constructor();
					BuildInstance(instance!, s, new Stack<Type>(), typeof(T));
					return new Result<T>(instance, s);
				};
		}

		private static Generator<object> One(Type type, Stack<Type> generationStack)
		{
			return
				s =>
				{
					var instance = CreateInstance(s, type);
					BuildInstance(instance, s, generationStack, type);
					return new Result<object>(instance, s);
				};
		}

		private static object CreateInstance(State state, Type type)
		{
			var typeToGenerate = GetTypeToGenerate(state, type);

			// If we have a registered constructor generator, use it
			if (state.Constructors.TryGetValue(typeToGenerate, out var constructors) && constructors.Count > 0)
			{
				var index = state.Random.Next(0, constructors.Count);
				var chosen = constructors[index];
				var instance = chosen(state);

				ValidateInstanceType(instance, typeToGenerate);
				return instance;
			}

			// Fallback to default constructor
			var defaultCtor = typeToGenerate
				.GetConstructors(MyBinding.Flags)
				.FirstOrDefault(c => c.GetParameters().Length == 0);

			if (defaultCtor == null)
				throw new InvalidOperationException($"No constructor or Construct(...) rule found for type {typeToGenerate}");

			var defaultInstance = defaultCtor.Invoke(Array.Empty<object>());
			ValidateInstanceType(defaultInstance, typeToGenerate);
			return defaultInstance;
		}


		private static void ValidateInstanceType(object instance, Type declaredType)
		{
			var actualType = instance?.GetType();
			if (actualType is not null &&
				actualType != declaredType &&
				!declaredType.IsAssignableFrom(actualType))
			{
				throw new InvalidOperationException(
					$"MGen created an instance of type '{actualType}' " +
					$"but expected a type assignable to '{declaredType}'. " +
					$"This might be due to an internal framework subclass " +
					$"like 'JsonValueCustomized<T>'. Consider using IgnoreAll() " +
					$"on '{actualType.BaseType}' or normalizing the type.");
			}
		}


		private static Type GetTypeToGenerate(State s, Type type)
		{
			var typeToGenerate = type;
			if (s.InheritanceInfo.ContainsKey(typeToGenerate))
			{
				var derivedTypes = s.InheritanceInfo[typeToGenerate];
				var index = s.Random.Next(0, derivedTypes.Count);
				typeToGenerate = derivedTypes[index];
			}
			return typeToGenerate;
		}

		private static void BuildInstance(object instance, State state, Stack<Type> generationStack, Type declaringType)
		{
			if (!state.StuffToIgnoreAll.Contains(declaringType))
				FillProperties(instance, state, generationStack);
			ApplyRegisteredActions(instance, state);
		}

		private static void FillProperties(object instance, State state, Stack<Type> generationStack)
		{
			foreach (var propertyInfo in instance.GetType().GetProperties(MyBinding.Flags))
			{
				HandleProperty(instance, state, propertyInfo, generationStack);
			}
		}

		private static void ApplyRegisteredActions(object instance, State state)
		{
			foreach (var key in state.ActionsToApply.Keys.Where(t => t.IsAssignableFrom(instance.GetType())))
			{
				foreach (var action in state.ActionsToApply[key])
				{
					action(state, instance);
				}
			}
		}

		private static void HandleProperty(object instance, State state, PropertyInfo propertyInfo, Stack<Type> generationStack)
		{
			if (NeedsToBeIgnored(state, propertyInfo))
				return;

			if (NeedsToBeCustomized(state, propertyInfo))
			{
				CustomizeProperty(instance, propertyInfo, state);
				return;
			}

			if (IsAKnownPrimitive(state, propertyInfo))
			{
				SetPrimitive(instance, propertyInfo, state);
				return;
			}

			if (IsAComponent(state, propertyInfo))
			{
				SetComponent(instance, propertyInfo, state, generationStack);
				return;
			}

			if (propertyInfo.PropertyType.IsEnum)
			{
				SetEnum(state, propertyInfo, instance);
				return;
			}

			if (IsANullableEnum(propertyInfo))
			{
				SetNullableEnum(state, propertyInfo, instance);
				return;
			}
		}

		private static bool NeedsToBeIgnored(State state, PropertyInfo propertyInfo)
		{
			return
				state
					.StuffToIgnore
					.Any(
						info => info.ReflectedType!.IsAssignableFrom(propertyInfo.ReflectedType)
								&& info.Name == propertyInfo.Name);
		}

		private static bool NeedsToBeCustomized(State state, PropertyInfo propertyInfo)
		{
			return
				state
					.Customizations
					.Keys
					.Any(
						info => info.ReflectedType!.IsAssignableFrom(propertyInfo.ReflectedType)
								&& info.Name == propertyInfo.Name);
		}

		private static void CustomizeProperty(object target, PropertyInfo propertyInfo, State state)
		{
			var key =
				state
					.Customizations
					.Keys
					.First(
						info => info.ReflectedType!.IsAssignableFrom(propertyInfo.ReflectedType)
								&& info.Name == propertyInfo.Name);
			var generator = state.Customizations[key];
			SetPropertyValue(propertyInfo, target, generator(state).Value);
		}

		private static bool IsAKnownPrimitive(State state, PropertyInfo propertyInfo)
		{
			return state.PrimitiveGenerators.ContainsKey(propertyInfo.PropertyType);
		}

		private static void SetPrimitive(object target, PropertyInfo propertyInfo, State state)
		{
			var generator = state.PrimitiveGenerators[propertyInfo.PropertyType];
			SetPropertyValue(propertyInfo, target, generator(state).Value);
		}

		private static bool IsAComponent(State state, PropertyInfo propertyInfo)
		{
			return state.Components.Contains(propertyInfo.PropertyType);
		}

		private static void SetComponent(object target, PropertyInfo propertyInfo, State state, Stack<Type> generationStack)
		{
			var type = propertyInfo.PropertyType;

			var currentDepth = generationStack.Count(t => t == type);

			if (state.RecursionRules.TryGetValue(type, out var rule))
			{
				if (rule.FallbackType != null)
				{
					if (currentDepth >= rule.MaxDepth - 2) // it really is - 2 just check the doc and come up with a cool name after beers
					{
						type = rule.FallbackType;
					}
				}
				else
				{
					if (currentDepth >= rule.MaxDepth - 1)
					{
						return; // This is the 'null' endpoint during recursive gen. Bad code style though.
					}
				}
			}
			else if (currentDepth >= 1)
			{
				return;
			}

			generationStack.Push(type);
			try
			{
				var result = One(type, generationStack)(state);
				SetPropertyValue(propertyInfo, target, result.Value);
			}
			finally
			{
				generationStack.Pop();
			}
		}

		private static void SetEnum(State state, PropertyInfo propertyInfo, object instance)
		{
			var value = GetEnumValue(propertyInfo.PropertyType, state);
			SetPropertyValue(propertyInfo, instance, value);
		}

		private static bool IsANullableEnum(PropertyInfo propertyInfo)
		{
			if (!propertyInfo.PropertyType.IsGenericType)
				return false;
			var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
			if (genericType != typeof(Nullable<>))
				return false;
			var genericArgument = propertyInfo.PropertyType.GetGenericArguments()[0];
			return genericArgument.IsEnum;
		}

		private static void SetNullableEnum(State state, PropertyInfo propertyInfo, object instance)
		{
			if (state.Random.Next(0, 5) == 0)
			{
				SetPropertyValue(propertyInfo, instance, null!);
			}
			else
			{
				var genericArgument = propertyInfo.PropertyType.GetGenericArguments()[0];
				var value = GetEnumValue(genericArgument, state);
				SetPropertyValue(propertyInfo, instance, System.Enum.ToObject(genericArgument, value));
			}
		}

		private static void SetPropertyValue(PropertyInfo propertyInfo, object target, object value)
		{
			var prop = propertyInfo;
			if (!prop.CanWrite)
				prop = propertyInfo.DeclaringType!.GetProperty(propertyInfo.Name);

			if (prop != null && prop.CanWrite) // todo check this
				prop.SetValue(target, value, null);
		}
	}
}