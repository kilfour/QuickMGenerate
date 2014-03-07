using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T> Apply<T>(this Generator<T> generator, Action<T> action)
		{
			return
				s =>
					{
						var result = generator(s);
						action(result.Value);
						return result;
					};
		}

		public static Generator<T> Apply<T>(this Generator<T> generator, Func<T, T> func)
		{
			return
				s =>
				{
					var result = generator(s);
					return new Result<T>(func(result.Value), s);
				};
		}
	}
}