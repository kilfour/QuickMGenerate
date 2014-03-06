using System;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[Decimals(
		Content = "Use `MGen.Decimal()`.",
		Order = 0)]
	public class DecimalGeneration
	{
		[Fact]
		[Decimals(
			Content = "The overload `MGen.Decimal(int min, int max)` generates an int higher or equal than min and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.Decimal(0, 0);
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate(state));
			}
		}

		[Fact]
		[Decimals(
			Content = "The default generator is (min = 1, max = 100).",
			Order = 2)]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Decimal();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
				Assert.True(val > 1);
				Assert.True(val < 100);
			}
		}

		[Fact]
		[Decimals(
			Content = "Can be made to return `decimal?` using the `.Nullable()` extension.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.Decimal().Nullable();
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
		[Decimals(
			Content = " - `decimal` is automatically detected and generated for object properties.",
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
		[Decimals(
			Content = " - `decimal?` is automatically detected and generated for object properties.",
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
			public decimal AProperty { get; set; }
			public decimal? ANullableProperty { get; set; }
		}

		public class DecimalsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public DecimalsAttribute()
			{
				Caption = "Decimals.";
				CaptionOrder = 5;
			}
		}
	}
}