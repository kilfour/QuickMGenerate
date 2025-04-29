using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<decimal> Decimal()
		{
			return Decimal(1, 100);
		}

		public static Generator<decimal> Decimal(decimal min, decimal max)
		{
			if (min > max)
				throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

			return s => new Result<decimal>(((decimal)s.Random.NextDouble() * (max - min)) + min, s);
		}
	}
}