using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class ReplacingTheDefaultGeneration
	{
		[Fact]
		public void UsesReplacement()
		{
			var generator =
				from _ in MGen.Replace(MGen.Int(42, 42))
				from result in MGen.One<SomeThingToGenerate>()
				select result;
			
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(42, generator.Generate(state).AnInt);
			}
		}

		[Fact]
		public void MultipleReplacements()
		{
			var generator =
				from replace1 in MGen.Replace(MGen.Int(42, 42))
				from result1 in MGen.One<SomeThingToGenerate>()
				from replace2 in MGen.Replace(MGen.Int(666, 666))
				from result2 in MGen.One<SomeThingToGenerate>()
				select new[] {result1, result2};

			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var array = generator.Generate(state);
				Assert.Equal(42, array[0].AnInt);
				Assert.Equal(666, array[1].AnInt);
			}
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
		}
	}
}