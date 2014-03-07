using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<float> Float()
		{
			return Float(1, 100);
		}

		public static Generator<float> Float(float min, float max)
		{
			return s => new Result<float>(((float)s.Random.NextDouble() * (max - min)) + min, s);
		}
	}
}