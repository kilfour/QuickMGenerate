using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class IgnoringProperties
	{
		[Fact]
		public void StaysDefaultValue()
		{
			var generator =
				from _ in MGen.With<SomeThingToGenerate>().Ignore(s => s.AnInt)
				from result in MGen.One<SomeThingToGenerate>()
				select result;

			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate(state).AnInt);
			}
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
		}
	}
}