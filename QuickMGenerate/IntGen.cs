namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, int> Int()
		{
			return Int(1, 100);
		}

		public static Generator<State, int> Int(int min, int max)
		{
			return s => new Result<State, int>(s.Random.Next(min, max), s);
		}
	}
}