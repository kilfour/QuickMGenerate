using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, string> String()
		{
			return s => new Result<State, string>("Hello", s);
		}
	}
}