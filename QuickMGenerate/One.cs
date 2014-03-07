using System;
using System.Linq;
using System.Reflection;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T> One<T>()
		{
			return
				s =>
					{
						var instance = Activator.CreateInstance<T>();
						foreach (var propertyInfo in instance.GetType().GetProperties(MyBinding.Flags))
						{
							if(NeedsToBeIgnored(s, propertyInfo))
								continue;

							if (IsAKnownPrimitive(s, propertyInfo))
							{
								SetPrimitive(instance, propertyInfo, s);
								continue;
							}

							if (IsAComponent(s, propertyInfo))
							{
								SetComponent(instance, propertyInfo, s);
								continue;
							}

							if (propertyInfo.PropertyType.IsEnum)
							{
								var value = GetEnumValue(propertyInfo.PropertyType, s);
								SetPropertyValue(propertyInfo, instance, value);
								continue;
							}
							if (propertyInfo.PropertyType.IsGenericType)
							{
								var genericType = propertyInfo.PropertyType.GetGenericTypeDefinition();
								if(genericType != typeof(Nullable<>))
									continue;
								var genericArgument = propertyInfo.PropertyType.GetGenericArguments()[0];
								if(!genericArgument.IsEnum)
									continue;
								if (s.Random.Next(0, 5) == 0)
								{
									SetPropertyValue(propertyInfo, instance, null);	
								}
								else
								{
									var value = GetEnumValue(genericArgument, s);
									SetPropertyValue(propertyInfo, instance, System.Enum.ToObject(genericArgument, value));	
								}
								
								continue;
							}
						}
						return new Result<State, T>(instance, s);
					};
		}

		private static bool NeedsToBeIgnored(State s, PropertyInfo propertyInfo)
		{
			return
				s.StuffToIgnore
					.Any(
						info => info.ReflectedType.IsAssignableFrom(propertyInfo.ReflectedType)
						        && info.Name == propertyInfo.Name);
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
			return state.Components.ContainsKey(propertyInfo.PropertyType);
		}

		private static void SetComponent(object target, PropertyInfo propertyInfo, State state)
		{
			var generator = state.Components[propertyInfo.PropertyType];
			SetPropertyValue(propertyInfo, target, generator(state).Value);
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