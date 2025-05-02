using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<object> AsObject<T>(this Generator<T> generator)
		{
			return s => new Result<object>(generator(s).Value!, s);
		}

		public static Generator<string> AsString<T>(this Generator<T> generator)
		{
			return s => new Result<string>(generator(s).Value!.ToString()!, s);
		}
	}
}