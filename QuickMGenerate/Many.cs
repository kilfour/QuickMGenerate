using System.Collections.Generic;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, IEnumerable<T>> Many<T>(this Generator<State, T> generator, int number)
		{
			return
				s =>
					{
						var list = new T[number];
						for (int i = 0; i < number; i++)
						{
							list[i] = generator(s).Value;
						}
						return new Result<State, IEnumerable<T>>(list, s);
					};

		}
	}
}