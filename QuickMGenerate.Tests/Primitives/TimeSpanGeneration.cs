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
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
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
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
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
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 30; i++)
			{
				var value = generator.Generate();
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
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate().AProperty.Ticks);
			}
		}

		[Fact]
		[TimeSpans(
			Content = " - `TimeSpan?` is automatically detected and generated for object properties.",
			Order = 5)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 30; i++)
			{
				var value = generator.Generate().ANullableProperty;
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