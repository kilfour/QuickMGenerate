using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class IntGeneration
	{
		[Fact]
		public void Zero()
		{
			var generator = MGen.Int(0, 0);
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate(state));
			}
		}

		[Fact]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Int();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state));
			}
		}

		[Fact]
		public void Nullable()
		{
			var generator = MGen.Int().Nullable();
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
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 10; i++)
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
			public int AProperty { get; set; }
			public int? ANullableProperty { get; set; }
		}
	}
}