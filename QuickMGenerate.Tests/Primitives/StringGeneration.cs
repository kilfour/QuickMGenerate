using System.Linq;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[Strings(
		Content = "Use `MGen.String()`. \n\nNo overload Exists.",
		Order = 0)]
	public class StringGeneration
	{
		[Fact]
		[Strings(
			Content = 
"The default generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.",
			Order = 1)]
		public void DefaultGeneratorStringElementsAlwaysBetweenLowerCaseAAndLowerCaseZ()
		{
			var valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			var generator = MGen.String();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
				Assert.True(val.All(s => valid.Any(c => c == s)), val);
			}
		}

		[Fact]
		[Strings(
			Content = "The Default generator generates a string of length higher than 0 and lower than 10.",
			Order = 2)]
		public void DefaultGeneratorStringIsBetweenOneAndTen()
		{
			var generator = MGen.String();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
				Assert.True(val.Length > 0);
				Assert.True(val.Length < 10);
			}
		}

		[Fact]
		[Strings(
			Content = " - `string` is automatically detected and generated for object properties.",
			Order = 3)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var value = generator.Generate(state).AProperty;
				Assert.NotNull(value);
				Assert.NotEqual("", value);
			}
		}

		public class SomeThingToGenerate
		{
			public string AProperty { get; set; }
		}

		public class StringsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public StringsAttribute()
			{
				Caption = "Strings";
				CaptionOrder = 2;
			}
		}
	}
}