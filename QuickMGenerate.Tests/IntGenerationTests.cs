using Xunit;

namespace QuickMGenerate.Tests
{
	public class IntGenerationTests
	{
		[Fact]
		public void Zero()
		{
			var generator = MGen.Int(0, 0);
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate());
			}
		}

		[Fact]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Int();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate());
			}
		}
	}
}