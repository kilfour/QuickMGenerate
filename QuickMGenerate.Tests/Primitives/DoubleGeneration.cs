namespace QuickMGenerate.Tests.Primitives
{
	[Doubles(
		Content = "Use `MGen.Double()`.",
		Order = 0)]
	public class DoubleGeneration
	{
		[Fact]
		[Doubles(
			Content = "The overload `MGen.Double(double min, double max)` generates a double higher or equal than min and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.Double(0, 0);
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate());
			}
		}

		[Fact]
		[Doubles(
			Content = "Throws an ArgumentException if min > max.",
			Order = 1.1)]
		public void Throws()
		{
			Assert.Throws<ArgumentException>(() => MGen.Double(1, 0).Generate());
		}

		[Fact]
		[Doubles(
			Content = "The default generator is (min = 1, max = 100).",
			Order = 2)]
		public void DefaultGeneratorBetweenOneAndHundred()
		{
			var generator = MGen.Double();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val >= 1);
				Assert.True(val < 100);
			}
		}

		[Fact]
		[Doubles(
			Content = "Can be made to return `double?` using the `.Nullable()` extension.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.Double().Nullable();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 50; i++)
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
		[Doubles(
			Content = " - `double` is automatically detected and generated for object properties.",
			Order = 4)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate().AProperty);
			}
		}

		[Fact]
		[Doubles(
			Content = " - `double?` is automatically detected and generated for object properties.",
			Order = 5)]
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
			public double AProperty { get; set; }
			public double? ANullableProperty { get; set; }
		}

		public class DoublesAttribute : ThePrimitiveGeneratorsAttribute
		{
			public DoublesAttribute()
			{
				Caption = "Doubles.";
				CaptionOrder = 8;
			}
		}
	}
}