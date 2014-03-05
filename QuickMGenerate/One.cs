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
							if (!IsAKnownPrimitive(s, propertyInfo))
								continue;
							SetPrimitive(instance, propertyInfo, s);
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