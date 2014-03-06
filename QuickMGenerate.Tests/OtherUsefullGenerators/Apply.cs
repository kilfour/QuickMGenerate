using System;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Apply(
		Content = "Use the `.Apply<T>(Func<T, T> action)` extension method.",
		Order = 0)]
	public class Apply
	{
		[Fact]
		[Apply(
			Content =
@"Applies the specified Function to the generated value, returning the result.
F.i. `MGen.Constant(41).Apply(i =>  i + 1)` will return 42.",
			Order = 1)]
		public void FunctionIsApplied()
		{
			var generator = MGen.Constant(41).Apply(i =>  i + 1);
			Assert.Equal(42, generator.Generate());
		}

		[Fact]
		[Apply(
			Content =
@"Par example, when you want all decimals to be rounded to a certain precision : 
```
var generator = 
	from _ in MGen.Decimal().Apply(d => Math.Round(d, 2)).Replace()
	from result in MGen.One<SomeThingToGenerate>()
	select result;
```",
			Order = 2)]
		public void RoundingExample()
		{
			var generator = 
				from _ in MGen.Decimal().Apply(d => Math.Round(d, 2)).Replace()
				from result in MGen.One<SomeThingToGenerate>()
				select result;
			var count = BitConverter.GetBytes(decimal.GetBits(generator.Generate().MyProperty)[3])[2];
			Assert.Equal(2, count);
		}

		[Fact]
		[Apply(
			Content =
@"An overload exists with signature `Apply<T>(Action<T> action)`.
This is usefull when dealing with objects and you just don't want to return said object.
E.g. `MGen.One<SomeThingToGenerate>().Apply(session.Save)`.",
			Order = 3)]
		public void ActionIsApplied()
		{
			var generator = MGen.One<SomeThingToGenerate>().Apply(thing => thing.MyProperty = 42);
			Assert.Equal(42, generator.Generate().MyProperty);
		}

		public class SomeThingToGenerate
		{
			public decimal MyProperty { get; set; }
		}

		public class ApplyAttribute : OtherUsefullGeneratorsAttribute
		{
			public ApplyAttribute()
			{
				Caption = "Apply.";
				CaptionOrder = 3;
			}
		}
	}
}