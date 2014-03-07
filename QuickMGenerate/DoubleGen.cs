using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, double> Double()
		{
			return Double(1, 100);
		}

		public static Generator<State, double> Double(double min, double max)
		{
			return s => new Result<State, double>((s.Random.NextDouble() * (max - min)) + min, s);
		}
	}
}