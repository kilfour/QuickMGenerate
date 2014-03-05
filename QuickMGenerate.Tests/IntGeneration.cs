using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class IntGeneration
	{
		[Fact]
		public void Zero()
		{
			var generator = MGen.Int(0, 0);
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate(state));
			}
		}

		[Fact]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Int();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state));
			}
		}
	}
}