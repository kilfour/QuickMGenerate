namespace QuickMGenerate.Tests.Primitives
{
	[Floats(
		Content = "Use `MGen.Float()`.",
		Order = 0)]
	public class FloatGeneration
	{
		[Fact]
		[Floats(
			Content = "The overload `MGen.Float(float min, float max)` generates a float higher or equal than min and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.Float(0, 0);
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate());
			}
		}

		[Fact]
		[Floats(
			Content = "The default generator is (min = 1, max = 100).",
			Order = 2)]
		public void DefaultGeneratorBetweenOneAndHundred()
		{
			var generator = MGen.Float();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val >= 1);
				Assert.True(val < 100);
			}
		}

		[Fact]
		[Floats(
			Content = "Can be made to return `float?` using the `.Nullable()` extension.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.Float().Nullable();
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
		[Floats(
			Content = " - `float` is automatically detected and generated for object properties.",
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
		[Floats(
			Content = " - `float?` is automatically detected and generated for object properties.",
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
			public float AProperty { get; set; }
			public float? ANullableProperty { get; set; }
		}

		public class FloatsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public FloatsAttribute()
			{
				Caption = "Floats.";
				CaptionOrder = 9;
			}
		}
	}
}