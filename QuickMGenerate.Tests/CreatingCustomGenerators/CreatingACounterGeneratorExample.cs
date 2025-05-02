using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.CreatingCustomGenerators
{
	public class CreatingACounterGeneratorExample
	{
		[Fact]
		public void LikeSo()
		{
			var generator =
				from s in MGen.Constant("SomeString")
				from c in Counter()
				from defaultString in MGen.Constant(s + c).Replace()
				from thing in MGen.One<SomethingToGenerate>()
				select thing;

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
			public string? MyProperty { get; set; }
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