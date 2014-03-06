using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T> Constant<T>(T value)
		{
			return s => new Result<State, T>(value, s);
		}
	}
}