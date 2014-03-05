using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class BoolGeneration
	{
		[Fact]
		public void DefaultGeneratorSometimesGeneratesTrue()
		{
			var generator = MGen.Bool();
			var state = new State();
			var isTrue = false;
			for (int i = 0; i < 10; i++)
			{
				isTrue = isTrue || generator.Generate(state);
			}
			Assert.True(isTrue);
		}

		[Fact]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isTrue = false;
			for (int i = 0; i < 10; i++)
			{
				isTrue = isTrue || generator.Generate(state).ABool;
			}
			Assert.True(isTrue);
		}

		public class SomeThingToGenerate
		{
			public bool ABool { get; set; }
		}
	}
}