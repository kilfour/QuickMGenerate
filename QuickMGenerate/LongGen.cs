using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<long> Long()
		{
			return Long(1, 100);
		}

		public static Generator<long> Long(long min, long max)
		{
			if (min > max)
				throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");

			return s => new Result<long>((long)((s.Random.NextDouble() * (max - min)) + min), s);
		}
	}
}