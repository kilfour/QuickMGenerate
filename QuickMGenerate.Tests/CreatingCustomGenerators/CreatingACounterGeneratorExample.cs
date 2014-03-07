using System.Linq;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.CreatingCustomGenerators
{
	public class CreatingACounterGeneratorExample
	{
		[Fact]
		public void Works()
		{
			var generator =
				from _ in MGen.Constant("SomeString").Append(Counter().AsString()).Replace()
				from s in MGen.One<SomethingToGenerate>()
				select s;

			var values = generator.Many(6).Generate().ToArray();

			Assert.Equal("SomeString1", values[0].MyProperty);
			Assert.Equal("SomeString2", values[1].MyProperty);
			Assert.Equal("SomeString3", values[2].MyProperty);
			Assert.Equal("SomeString4", values[3].MyProperty);
			Assert.Equal("SomeString5", values[4].MyProperty);
			Assert.Equal("SomeString6", values[5].MyProperty);
		}

		public class SomethingToGenerate
		{
			public string MyProperty { get; set; }
		}

		public Generator<int> Counter()
		{
			return
				state =>
					{
						var counter = state.Get("MyCounter", 0);
						var newVal = counter + 1;
						state.Set("MyCounter", newVal);
						return new Result<int>(newVal, state);
					};
		}
	}
}