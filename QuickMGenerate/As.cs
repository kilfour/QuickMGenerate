using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, object> AsObject<T>(this Generator<State, T> generator)
		{
			return s => new Result<State, object>(generator(s).Value, s);
		}

		public static Generator<State, string> AsString<T>(this Generator<State, T> generator)
		{
			return s => new Result<State, string>(generator(s).Value.ToString(), s);
		}
	}
}