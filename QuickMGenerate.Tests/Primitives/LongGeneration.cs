using System;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[Longs(
		Content = "Use `MGen.Long()`.",
		Order = 0)]
	public class LongGeneration
	{
		[Fact]
		[Longs(
			Content = "The overload `MGen.Long(long min, long max)` generates a long higher or equal than min and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.Long(0, 0);
			var state = new State();
			for (long i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate(state));
			}
		}

		[Fact]
		[Longs(
			Content = "The default generator is (min = 1, max = 100).",
			Order = 2)]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Long();
			var state = new State();
			for (long i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state));
			}
		}

		[Fact]
		[Longs(
			Content = "Can be made to return `long?` using the `.Nullable()` extension.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.Long().Nullable();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (long i = 0; i < 20; i++)
			{
				var value = generator.Generate(state);
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(0, value.Value);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		[Fact]
		[Longs(
			Content = " - `long` is automatically detected and generated for object properties.",
			Order = 4)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (long i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state).AProperty);
			}
		}

		[Fact]
		[Longs(
			Content = " - `Int64` is automatically detected and generated for object properties.",
			Order = 5)]
		public void Long32Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (long i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state).AnInt64Property);
			}
		}

		[Fact]
		[Longs(
			Content = " - `long?` is automatically detected and generated for object properties.",
			Order = 6)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (long i = 0; i < 20; i++)
			{
				var value = generator.Generate(state).ANullableProperty;
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(0, value.Value);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		public class SomeThingToGenerate
		{
			public long AProperty { get; set; }
			public Int64 AnInt64Property { get; set; }
			public long? ANullableProperty { get; set; }
		}

		public class LongsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public LongsAttribute()
			{
				Caption = "Longs";
				CaptionOrder = 7;
			}
		}
	}
}