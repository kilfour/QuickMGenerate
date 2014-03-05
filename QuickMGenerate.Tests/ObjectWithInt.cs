using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class ObjectWithInt
	{
		[Fact]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state).AnInt);
			}
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
		}
	}
}