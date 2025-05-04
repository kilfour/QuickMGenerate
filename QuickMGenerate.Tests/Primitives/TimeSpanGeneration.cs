using QuickMGenerate.Tests._Tools;

namespace QuickMGenerate.Tests.Primitives;

[TimeSpans(
	Content = "Use `MGen.TimeSpan()`.",
	Order = 0)]
public class TimeSpanGeneration
{
	[Fact]
	[TimeSpans(
		Content = "The overload `MGen.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.",
		Order = 1)]
	public void OverloadRange()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(MGen.TimeSpan(5),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 5", a => a.Ticks < 5));
	}

	[Fact]
	[TimeSpans(
		Content = "The default generator is (max = 1000).",
		Order = 2)]
	public void GeneratesValuesBetweenOneIncludedAndThousandExcluded()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(MGen.TimeSpan(),
			("Ticks >= 1", a => a.Ticks >= 1), ("Ticks < 1000", a => a.Ticks < 1000));
	}

	[Fact]
	[TimeSpans(
		Content = "Can be made to return `TimeSpan?` using the `.Nullable()` combinator.",
		Order = 3)]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(MGen.TimeSpan().Nullable());
	}

	[Fact]
	[TimeSpans(
		Content = " - `TimeSpan` is automatically detected and generated for object properties.",
		Order = 4)]
	public void Property()
	{
		CheckIf.GeneratedValuesShouldAllSatisfy(
			MGen.One<SomeThingToGenerate>().Select(a => a.AProperty),
			("not zero", a => a.Ticks != 0));
	}

	[Fact]
	[TimeSpans(
		Content = " - `TimeSpan?` is automatically detected and generated for object properties.",
		Order = 5)]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			MGen.One<SomeThingToGenerate>().Select(a => a.ANullableProperty));
	}

	public class SomeThingToGenerate
	{
		public TimeSpan AProperty { get; set; }
		public TimeSpan? ANullableProperty { get; set; }
	}

	public class TimeSpansAttribute : ThePrimitiveGeneratorsAttribute
	{
		public TimeSpansAttribute()
		{
			Caption = "TimeSpans.";
			CaptionOrder = 12;
		}
	}
}