using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T> Constant<T>(T value)
		{
			return s => new Result<T>(value, s);
		}
	}
}