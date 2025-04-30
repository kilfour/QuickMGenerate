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
			for (long i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate());
			}
		}

		[Fact]
		[Longs(
			Content = "Throws an ArgumentException if min > max.",
			Order = 1.1)]
		public void Throws()
		{
			Assert.Throws<ArgumentException>(() => MGen.Long(1, 0).Generate());
		}

		[Fact]
		[Longs(
			Content = "The default generator is (min = 1, max = 100).",
			Order = 2)]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Long();
			for (long i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate());
			}
		}

		[Fact]
		[Longs(
			Content = "Can be made to return `long?` using the `.Nullable()` combinator.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.Long().Nullable();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (long i = 0; i < 50; i++)
			{
				var value = generator.Generate();
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
			for (long i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate().AProperty);
			}
		}

		[Fact]
		[Longs(
			Content = " - `Int64` is automatically detected and generated for object properties.",
			Order = 5)]
		public void Long32Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			for (long i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate().AnInt64Property);
			}
		}

		[Fact]
		[Longs(
			Content = " - `long?` is automatically detected and generated for object properties.",
			Order = 6)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (long i = 0; i < 50; i++)
			{
				var value = generator.Generate().ANullableProperty;
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
				Caption = "Longs.";
				CaptionOrder = 7;
			}
		}
	}
}