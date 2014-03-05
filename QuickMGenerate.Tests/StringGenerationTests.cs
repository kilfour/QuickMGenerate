using System.Linq;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class StringGenerationTests
	{
		[Fact]
		public void DefaultGeneratorStringElementsAlwaysBetweenLowerCaseAAndLowerCaseZ()
		{
			var valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			var generator = MGen.String();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val.All(s => valid.Any(c => c == s)), val);
			}
		}

		[Fact]
		public void DefaultGeneratorStringIsNeverEmpty()
		{
			var generator = MGen.String();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val.Length > 0);
			}
		}

		[Fact]
		public void DefaultGeneratorStringIsSmallerThanTen()
		{
			var generator = MGen.String();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val.Length < 10);
			}
		}
	}
}