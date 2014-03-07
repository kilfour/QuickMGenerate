using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T> Modify<T>(this Generator<T> generator, T instance)
		{
			return
				s =>
					{

						foreach (var propertyInfo in instance.GetType().GetProperties(MyBinding.Flags))
						{
							if(s.StuffToIgnore.Contains(propertyInfo))
								continue;
							if (!IsAKnownPrimitive(s, propertyInfo))
								continue;
							var before = propertyInfo.GetValue(instance, null);
							var primitiveGenerator = s.PrimitiveGenerators[propertyInfo.PropertyType];
							var value = primitiveGenerator(s).Value;
							while(IsEqual(before, value))
								value = primitiveGenerator(s).Value;
							SetPropertyValue(propertyInfo, instance, value);
						}
						return new Result<T>(instance, s);
					};
		}

		private static bool IsEqual(object before, object after)
		{
			if (before == null)
			{
				if (after != null)
					return false;
			}
			else if (before.Equals(after))
				return true;
			return false;
		}
	}
}