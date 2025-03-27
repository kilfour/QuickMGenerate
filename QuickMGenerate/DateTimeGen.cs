using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<DateTime> DateTime()
		{
			return DateTime(new DateTime(1970, 1, 1), new DateTime(2020, 12, 31));
		}

		public static Generator<DateTime> DateTime(DateTime min, DateTime max)
		{
			return
				s =>
					{
						var ticks = (long)((s.Random.NextDouble() * (max.Ticks - min.Ticks)) + min.Ticks);
						var value = new DateTime(ticks);
						// why ???
						value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
						return new Result<DateTime>(value, s);
					};
		}
	}
}