using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<short> Short()
		{
			return Short(1, 100);
		}

		public static Generator<short> Short(short min, short max)
		{
			return s => new Result<short>((short)((s.Random.NextDouble() * (max - min)) + min), s);
		}
	}
}