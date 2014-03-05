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
							SetIfIsAKnownPrimitive(instance, propertyInfo, s);
						}
						return new Result<State, T>(instance, s);
					};
		}

		private static void SetIfIsAKnownPrimitive(object target, PropertyInfo propertyInfo, State state)
		{
			var primitiveGenerator = state.PrimitiveGenerators[propertyInfo.PropertyType];
			if (primitiveGenerator != null)
			{
				SetPropertyValue(propertyInfo, target, primitiveGenerator(state).Value);
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