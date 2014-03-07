using System;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[TimeSpans(
		Content = "Use `MGen.TimeSpan()`.",
		Order = 0)]
	public class TimeSpanGeneration
	{
		[Fact]
		[TimeSpans(
			Content = "The overload `MGen.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.TimeSpan(5);
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
				Assert.True(val.Ticks >= 1);
				Assert.True(val.Ticks < 5);
			}
		}

		[Fact]
		[TimeSpans(
			Content = "The default generator is (max = 1000).",
			Order = 2)]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.TimeSpan();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
				Assert.True(val.Ticks >= 1);
				Assert.True(val.Ticks < 1000);
			}
		}

		[Fact]
		[TimeSpans(
			Content = "Can be made to return `TimeSpan?` using the `.Nullable()` extension.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.TimeSpan().Nullable();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 30; i++)
			{
				var value = generator.Generate(state);
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(0, value.Value.Ticks);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		[Fact]
		[TimeSpans(
			Content = " - `TimeSpan` is automatically detected and generated for object properties.",
			Order = 4)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state).AProperty.Ticks);
			}
		}

		[Fact]
		[TimeSpans(
			Content = " - `TimeSpan?` is automatically detected and generated for object properties.",
			Order = 5)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 30; i++)
			{
				var value = generator.Generate(state).ANullableProperty;
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(0, value.Value.Ticks);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
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
}