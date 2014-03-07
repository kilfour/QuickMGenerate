using System.Collections.Generic;
using System.Linq;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T> ChooseFromThese<T>(params T[] values)
		{
			return ChooseFrom(values);
		}

		public static Generator<State, T> ChooseFrom<T>(IEnumerable<T> values)
		{
			return
				s =>
				{
					var index = Int(0, values.Count()).Generate(s);
					return new Result<State, T>(values.ElementAt(index), s);
				};
		}
	}
}