using System.Reflection;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T> Enum<T>()
			where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}

			return s => new Result<T>((T)GetEnumValue(typeof(T), s), s);
		}

		private static object GetEnumValue(Type type, State s)
		{
			var values = GetEnumValues(type);
			var index = Int(0, values.Count())(s).Value;
			return values.ElementAt(index);
		}

		private static IEnumerable<object> GetEnumValues(IReflect type)
		{
			return
				type
					.GetFields(BindingFlags.Public | BindingFlags.Static)
					.Select(i => i.GetRawConstantValue());
		}
	}
}