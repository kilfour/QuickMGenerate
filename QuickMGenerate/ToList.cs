using System.Collections.Generic;
using System.Linq;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<List<T>> ToList<T>(this Generator<IEnumerable<T>> generator)
		{
			return s => new Result<List<T>>(generator(s).Value.ToList(), s);
		}
	}
}