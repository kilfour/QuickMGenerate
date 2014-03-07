using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<double> Double()
		{
			return Double(1, 100);
		}

		public static Generator<double> Double(double min, double max)
		{
			return s => new Result<double>((s.Random.NextDouble() * (max - min)) + min, s);
		}
	}
}