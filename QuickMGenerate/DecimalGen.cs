using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, decimal> Decimal()
		{
			return Decimal(1, 100);
		}

		public static Generator<State, decimal> Decimal(decimal min, decimal max)
		{
			return s => new Result<State, decimal>(((decimal)s.Random.NextDouble() * (max - min)) + min, s);
		}
	}
}