using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.CreatingCustomGenerators
{
	[CustomGeneratorsExamples(
		Content =
@"Any function that returns a value of type `Generator<T>` can be used as a generator.

Generator is defined as a delegate like so :
```
public delegate IResult<TValue> Generator<out TValue>(State input)
```
",
		Order = 0)]
	public class CustomGenerators
	{
		[Fact]
		[CustomGeneratorsExamples(
			Content =
@"So f.i. to define a generator that always returns the number forty-two we need a function that returns the following :
```
return s => new Result<State, int>(42, s);
```",
			Order = 1)]
		public void CustomGeneratorExample()
		{
			Assert.Equal(42, Generate42().Generate());
		}

		public Generator<int> Generate42()
		{
			return s => new Result<int>(42, s);
		}

		[Fact]
		[CustomGeneratorsExamples(
			Content =
@"As you can see from the signature a state object is passed to the generator.
This is where the random seed lives.
If you want any kind of random, it is advised to use that one, like so :
```
return s => new Result<State, int>(s.Random.Next(42, 42), s);
```

See also : [Creating a counter generator](./QuickMGenerate.Tests/CreatingCustomGenerators/CreatingACounterGeneratorExample.cs).",
			Order = 2)]
		public void CustomGeneratorExampleWithRandom()
		{
			Assert.Equal(42, Generate42OtherWay().Generate());
		}

		public Generator<int> Generate42OtherWay()
		{
			return s => new Result<int>(s.Random.Next(42, 42), s);
		}

		public class CustomGeneratorsExamplesAttribute : CustomGeneratorsAttribute
		{
			public CustomGeneratorsExamplesAttribute()
			{
				Caption = "How To";
				CaptionOrder = 0;
			}
		}
	}
}