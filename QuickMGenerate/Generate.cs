using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static T Generate<T>(this Generator<T> generator)
		{
			return generator(new State()).Value;
		}

		public static T Generate<T>(this Generator<T> generator, State state)
		{
			return generator(state).Value;
		}
	}
}