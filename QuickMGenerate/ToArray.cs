using System.Collections.Generic;
using System.Linq;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<IEnumerable<T>> ToArray<T>(this Generator<IEnumerable<T>> generator)
		{
			return s => new Result<IEnumerable<T>>(generator(s).Value.ToArray(), s);
		}
	}
}