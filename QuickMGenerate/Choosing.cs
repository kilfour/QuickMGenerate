using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T> ChooseFrom<T>(params T[] values)
		{
			return
				s =>
					{
						var index = Int(0, values.Length).Generate(s);
						return new Result<State, T>(values[index], s);
					};
		}
	}
}