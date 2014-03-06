using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T> Apply<T>(this Generator<State, T> generator, Action<T> action)
		{
			return
				s =>
					{
						var result = generator(s);
						action(result.Value);
						return result;
					};
		}

		public static Generator<State, T> Apply<T>(this Generator<State, T> generator, Func<T, T> func)
		{
			return
				s =>
				{
					var result = generator(s);
					return new Result<State, T>(func(result.Value), s);
				};
		}
	}
}