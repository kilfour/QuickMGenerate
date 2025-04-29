using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<int> Int()
		{
			return Int(1, 100);
		}

		public static Generator<int> Int(int min, int max)
		{
			if (min > max)
				throw new ArgumentException($"Invalid range: min ({min}) > max ({max})");
			return s => new Result<int>(s.Random.Next(min, max), s);
		}
	}
}