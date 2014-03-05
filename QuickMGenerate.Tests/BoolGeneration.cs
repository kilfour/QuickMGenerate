using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class BoolGeneration
	{
		[Fact]
		public void DefaultGeneratorSometimesGeneratesTrue()
		{
			var generator = MGen.Bool();
			var state = new State();
			var isTrue = false;
			for (int i = 0; i < 10; i++)
			{
				isTrue = isTrue || generator.Generate(state);
			}
			Assert.True(isTrue);
		}

		[Fact]
		public void Nullable()
		{
			var generator = MGen.Bool().Nullable();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state);
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
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
			var isTrue = false;
			for (int i = 0; i < 10; i++)
			{
				isTrue = isTrue || generator.Generate(state).AProperty;
			}
			Assert.True(isTrue);
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
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		public class SomeThingToGenerate
		{
			public bool AProperty { get; set; }
			public bool? ANullableProperty { get; set; }
		}
	}
}