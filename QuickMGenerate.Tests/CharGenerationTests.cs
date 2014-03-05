using System.Linq;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class CharGenerationTests
	{
		[Fact]
		public void DefaultGeneratorAlwaysBetweenLowerCaseAAndLowerCaseZ()
		{
			var valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			var generator = MGen.Char();
			for (int i = 0; i < 100; i++)
			{
				var val = generator.Generate();
				Assert.True(valid.Any(c => c == val), val.ToString());
			}
		}

		[Fact]
		public void IsRandom()
		{
			var generator = MGen.Char();
			var val = generator.Generate();
			var differs = false;
			for (int i = 0; i < 10; i++)
			{
				if (val != generator.Generate())
					differs = true;
			}
			Assert.True(differs);
		}
	}
}