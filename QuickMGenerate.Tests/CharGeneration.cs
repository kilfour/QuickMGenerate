using System.Linq;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class CharGeneration
	{
		[Fact]
		public void DefaultGeneratorAlwaysBetweenLowerCaseAAndLowerCaseZ()
		{
			var valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			var generator = MGen.Char();
			var state = new State();
			for (int i = 0; i < 100; i++)
			{
				var val = generator.Generate(state);
				Assert.True(valid.Any(c => c == val), val.ToString());
			}
		}

		[Fact]
		public void IsRandom()
		{
			var generator = MGen.Char();
			var state = new State();
			var val = generator.Generate(state);
			var differs = false;
			for (int i = 0; i < 10; i++)
			{
				if (val != generator.Generate(state))
					differs = true;
			}
			Assert.True(differs);
		}
	}
}