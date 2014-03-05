using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static class GeneratorToLinq
	{
		public static Generator<TState, TValueTwo> Select<TState, TValueOne, TValueTwo>(
			this Generator<TState, TValueOne> generator,
			Func<TValueOne, TValueTwo> selector)
		{
			if (generator == null)
				throw new ArgumentNullException("generator");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return s => new Result<TState, TValueTwo>(selector(generator(s).Value), s);
		}

		public static Generator<TState, TValueTwo> SelectMany<TState, TValueOne, TValueTwo>(
			this Generator<TState, TValueOne> generator,
			Func<TValueOne, Generator<TState, TValueTwo>> selector)
		{
			if (generator == null)
				throw new ArgumentNullException("generator");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return s => selector(generator(s).Value)(s);
		}

		public static Generator<TState, TValueThree> SelectMany<TState, TValueOne, TValueTwo, TValueThree>(
			this Generator<TState, TValueOne> generator,
			Func<TValueOne, Generator<TState, TValueTwo>> selector,
			Func<TValueOne, TValueTwo, TValueThree> projector)
		{
			if (generator == null)
				throw new ArgumentNullException("generator");
			if (selector == null)
				throw new ArgumentNullException("selector");
			if (projector == null)
				throw new ArgumentNullException("projector");

			return generator.SelectMany(x => selector(x).Select(y => projector(x, y)));
		}
	}
}