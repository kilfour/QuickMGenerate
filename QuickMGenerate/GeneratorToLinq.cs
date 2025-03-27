using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static class GeneratorToLinq
	{
		public static Generator<TValueTwo> Select<TValueOne, TValueTwo>(
			this Generator<TValueOne> generator,
			Func<TValueOne, TValueTwo> selector)
		{
			if (generator == null)
				throw new ArgumentNullException("generator");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return s => new Result<TValueTwo>(selector(generator(s).Value), s);
		}

		// This is the Bind function
		public static Generator<TValueTwo> SelectMany<TValueOne, TValueTwo>(
			this Generator<TValueOne> generator,
			Func<TValueOne, Generator<TValueTwo>> selector)
		{
			if (generator == null)
				throw new ArgumentNullException("generator");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return s => selector(generator(s).Value)(s);
		}

		public static Generator<TValueThree> SelectMany<TValueOne, TValueTwo, TValueThree>(
			this Generator<TValueOne> generator,
			Func<TValueOne, Generator<TValueTwo>> selector,
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