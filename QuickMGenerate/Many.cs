using System.Collections.Generic;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<IEnumerable<T>> Many<T>(this Generator<T> generator, int number)
		{
			return s => new Result<IEnumerable<T>>(GetEnumerable(number, generator, s), s);
		}

		private static IEnumerable<T> GetEnumerable<T>(int number, Generator<T> generator, State s)
		{
			for (int i = 0; i < number; i++)
			{
				yield return generator(s).Value;
			}
		}
	}
}