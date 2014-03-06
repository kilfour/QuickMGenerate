using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[Booleans(
		Content = "Use `MGen.Bool()`. \n\nNo overload Exists.", 
		Order = 0)]
	public class BoolGeneration
	{
		[Fact]
		[Booleans(
			Content = "The default generator generates True or False.", 
			Order = 1)]
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
		[Booleans(
			Content = "Can be made to return `bool?` using the `.Nullable()` extension.", 
			Order = 2)]
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
		[Booleans(
			Content = " - `bool` is automatically detected and generated for object properties.",
			Order = 3)]
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
		[Booleans(
			Content = " - `bool?` is automatically detected and generated for object properties.",
			Order = 4)]
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

		public class BooleansAttribute : GeneratingPrimitivesAttribute
		{
			public BooleansAttribute()
			{
				Caption = "Booleans";
				CaptionOrder = 3;
			}
		}
	}
}