using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T[]> ToArray<T>(this Generator<IEnumerable<T>> generator)
		{
			return s => new Result<T[]>(generator(s).Value.ToArray(), s);
		}
	}
}