using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<TimeSpan> TimeSpan()
		{
			return TimeSpan(1000);
		}

		public static Generator<TimeSpan> TimeSpan(int max)
		{
			return s => new Result<TimeSpan>(new TimeSpan(s.Random.Next(1, max)), s);
		}
	}
}