using System.Security.Cryptography;

namespace QuickMGenerate.UnderTheHood
{
	public class MoreRandom
	{
		private readonly Random _random;

		public MoreRandom(int seed)
		{
			_random = new Random(seed);
		}

		public int Next(int minimumValue, int maximumValue)
		{
			if (maximumValue <= minimumValue)
				return minimumValue;

			return _random.Next(minimumValue, maximumValue);
		}

		public double NextDouble()
		{
			return _random.NextDouble();
		}
	}
}