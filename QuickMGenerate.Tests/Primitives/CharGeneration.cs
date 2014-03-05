using System.Linq;
using QuickMGenerate.Tests.Tools;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[Chars(
		Content = "Use `MGen.Char()`. \n\nNo overload Exists.",
		Order = 0)]
	public class CharGeneration
	{
		private readonly char[] valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

		[Fact]
		[Chars(
			Content = "The default generator always generates a char between lower case 'a' and lower case 'z'.",
			Order = 1)]
		public void DefaultGeneratorAlwaysBetweenLowerCaseAAndLowerCaseZ()
		{
			var generator = MGen.Char();
			var state = new State();
			for (int i = 0; i < 100; i++)
			{
				var val = generator.Generate(state);
				Assert.True(valid.Any(c => c == val), val.ToString());
			}
		}

		[Fact]
		public void IsRandom()
		{
			var generator = MGen.Char();
			var state = new State();
			var val = generator.Generate(state);
			var differs = false;
			for (int i = 0; i < 10; i++)
			{
				if (val != generator.Generate(state))
					differs = true;
			}
			Assert.True(differs);
		}

		[Fact]
		[Chars(
			Content = "Can be made to return `char?` using the `.Nullable()` extension.",
			Order = 2)]
		public void Nullable()
		{
			var generator = MGen.Char().Nullable();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state);
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.True(valid.Any(c => c == value.Value), value.Value.ToString());
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		[Fact]
		[Chars(
			Content = " - `char` is automatically detected and generated for object properties.",
			Order = 3)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var value = generator.Generate(state).AProperty;
				Assert.True(valid.Any(c => c == value), value.ToString());
			}
		}

		[Fact]
		[Chars(
			Content = " - `char?` is automatically detected and generated for object properties.",
			Order = 4)]
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
					Assert.True(valid.Any(c => c == value.Value), value.Value.ToString());
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		public class SomeThingToGenerate
		{
			public char AProperty { get; set; }
			public char? ANullableProperty { get; set; }
		}

		public class CharsAttribute : GeneratingPrimitivesAttribute
		{
			public CharsAttribute()
			{
				Caption = "Chars";
				CaptionOrder = 1;
			}
		}
	}
}