using QuickMGenerate.UnderTheHood;
using Xunit;

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
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate(state));
			}
		}

		[Fact]
		[Floats(
			Content = "The default generator is (min = 1, max = 100).",
			Order = 2)]
		public void DefaultGeneratorBetweenOneAndHundred()
		{
			var generator = MGen.Float();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
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
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
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
		[Floats(
			Content = " - `float` is automatically detected and generated for object properties.",
			Order = 4)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state).AProperty);
			}
		}

		[Fact]
		[Floats(
			Content = " - `float?` is automatically detected and generated for object properties.",
			Order = 5)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
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