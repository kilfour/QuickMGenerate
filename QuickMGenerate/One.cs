using System;
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
							if(s.StuffToIgnore.Contains(propertyInfo))
								continue;
							if (IsAKnownPrimitive(s, propertyInfo))
							{
								SetPrimitive(instance, propertyInfo, s);
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

		private static bool IsAKnownPrimitive(State state, PropertyInfo propertyInfo)
		{
			return state.PrimitiveGenerators.ContainsKey(propertyInfo.PropertyType);
		}

		private static void SetPrimitive(object target, PropertyInfo propertyInfo, State state)
		{
			var primitiveGenerator = state.PrimitiveGenerators[propertyInfo.PropertyType];
			SetPropertyValue(propertyInfo, target, primitiveGenerator(state).Value);
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