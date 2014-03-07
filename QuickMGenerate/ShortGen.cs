using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, short> Short()
		{
			return Short(1, 100);
		}

		public static Generator<State, short> Short(short min, short max)
		{
			return s => new Result<State, short>((short)((s.Random.NextDouble() * (max - min)) + min), s);
		}
	}
}