using System;
using System.Collections.Generic;
using System.Reflection;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		private static readonly Dictionary<Type, Generator<State, object>> PrimitiveGenerators
			= new Dictionary<Type, Generator<State, object>>
			  	{
			  		{ typeof(int), Int().AsObject() }
				};

		public static Generator<State, T> One<T>()
		{
			return
				s =>
					{
						var instance = Activator.CreateInstance<T>();
						foreach (var propertyInfo in instance.GetType().GetProperties(MyBinding.Flags))
						{
							SetIfIsAKnownPrimitive(instance, propertyInfo, s);
						}
						return new Result<State, T>(instance, s);
					};
		}

		public static Generator<State, object> AsObject<T>(this Generator<State, T> generator)
		{
			return
				s =>
					{  
						var val = generator(s).Value;
						return new Result<State, object>(val, s);
					};
		}

		private static void SetIfIsAKnownPrimitive(object target, PropertyInfo propertyInfo, State state)
		{
			var primitiveGenerator = PrimitiveGenerators[propertyInfo.PropertyType];
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