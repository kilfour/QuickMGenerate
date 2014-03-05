using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, bool> Bool()
		{
			return s => new Result<State, bool>(s.Random.Next(0, 2) > 0, s);
		}
	}
}