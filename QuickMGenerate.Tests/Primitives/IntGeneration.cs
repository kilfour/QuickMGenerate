namespace QuickMGenerate.Tests.Primitives
{
	[Ints(
		Content = "Use `MGen.Int()`.",
		Order = 0)]
	public class IntGeneration
	{
		[Fact]
		[Ints(
			Content = "The overload `MGen.Int(int min, int max)` generates an int higher or equal than min and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.Int(0, 0);
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate());
			}
		}

		[Fact]
		[Ints(
			Content = "Throws an ArgumentException if min > max.",
			Order = 2)]
		public void Throws()
		{
			Assert.Throws<ArgumentException>(() => MGen.Int(1, 0).Generate());
		}

		[Fact]
		[Ints(
			Content = "The default generator is (min = 1, max = 100).",
			Order = 3)]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Int();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val >= 1);
				Assert.True(val < 100);
			}
		}

		[Fact]
		[Ints(
			Content = "Can be made to return `int?` using the `.Nullable()` combinator.",
			Order = 4)]
		public void Nullable()
		{
			var generator = MGen.Int().Nullable();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 100; i++)
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
		[Ints(
			Content = " - `int` is automatically detected and generated for object properties.",
			Order = 5)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate().AProperty);
			}
		}

		[Fact]
		[Ints(
			Content = " - `Int32` is automatically detected and generated for object properties.",
			Order = 6)]
		public void Int32Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate().AnInt32Property);
			}
		}

		[Fact]
		[Ints(
			Content = " - `int?` is automatically detected and generated for object properties.",
			Order = 7)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 50; i++)
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
			public int AProperty { get; set; }
			public Int32 AnInt32Property { get; set; }
			public int? ANullableProperty { get; set; }
		}

		public class IntsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public IntsAttribute()
			{
				Caption = "Integers.";
				CaptionOrder = 0;
			}
		}
	}
}