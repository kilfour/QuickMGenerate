using System;
using System.Linq;
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
						var instance = (T)CreateInstance(typeof(T));
						BuildInstance(instance, s);
						return new Result<T>(instance, s);
					};
		}

		public static Generator<T> One<T>(Func<T> constructor)
		{
			return
				s =>
				{
					var instance = constructor();
					BuildInstance(instance, s);
					return new Result<T>(instance, s);
				};
		}

		private static Generator<object> One(Type type)
		{
			return
				s =>
				{
					var instance = CreateInstance(type);
					BuildInstance(instance, s);
					return new Result<object>(instance, s);
				};
		}

		private static object CreateInstance(Type type)
		{
			var constructor =
				type
					.GetConstructors(MyBinding.Flags)
					.First(c => c.GetParameters().Count() == 0);

			return constructor.Invoke(new object[0]);
		}

		private static void BuildInstance(object instance, State state)
		{
			FillProperties(instance, state);
			ApplyRegisteredActions(instance, state);
		}

		private static void FillProperties(object instance, State state)
		{
			foreach (var propertyInfo in instance.GetType().GetProperties(MyBinding.Flags))
			{
				HandleProperty(instance, state, propertyInfo);
			}
		}

		private static void ApplyRegisteredActions(object instance, State state)
		{
			foreach (var key in state.ActionsToApply.Keys.Where(t => t.IsAssignableFrom(instance.GetType())))
			{
				foreach (var action in state.ActionsToApply[key])
				{
					action(instance);
				}
			}
		}

		private static void HandleProperty(object instance, State state, PropertyInfo propertyInfo)
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
				SetComponent(instance, propertyInfo, state);
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
						info => info.ReflectedType.IsAssignableFrom(propertyInfo.ReflectedType)
						        && info.Name == propertyInfo.Name);
		}

		private static bool NeedsToBeCustomized(State state, PropertyInfo propertyInfo)
		{
			return
				state
					.Customizations
					.Keys
					.Any(
						info => info.ReflectedType.IsAssignableFrom(propertyInfo.ReflectedType)
						        && info.Name == propertyInfo.Name);
		}

		private static void CustomizeProperty(object target, PropertyInfo propertyInfo, State state)
		{
			var key =
				state
					.Customizations
					.Keys
					.First(
						info => info.ReflectedType.IsAssignableFrom(propertyInfo.ReflectedType)
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

		private static void SetComponent(object target, PropertyInfo propertyInfo, State state)
		{
			SetPropertyValue(propertyInfo, target, One(propertyInfo.PropertyType)(state).Value);
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
				SetPropertyValue(propertyInfo, instance, null);
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
				prop = propertyInfo.DeclaringType.GetProperty(propertyInfo.Name);
			prop.SetValue(target, value, null);
		}
	}
}