using System.Collections.Generic;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, IEnumerable<T>> Many<T>(this Generator<State, T> generator, int number)
		{
			return s => new Result<State, IEnumerable<T>>(GetEnumerable(number, generator, s), s);
		}

		private static IEnumerable<T> GetEnumerable<T>(int number, Generator<State, T> generator, State s)
		{
			for (int i = 0; i < number; i++)
			{
				yield return generator(s).Value;
			}
		}
	}
}