namespace QuickMGenerate.Tests.Primitives
{
	[Strings(
		Content = "Use `MGen.String()`.",
		Order = 0)]
	public class StringGeneration
	{
		[Fact]
		[Strings(
			Content =
"The generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.",
			Order = 1)]
		public void DefaultGeneratorStringElementsAlwaysBetweenLowerCaseAAndLowerCaseZ()
		{
			var valid = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			var generator = MGen.String();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val.All(s => valid.Any(c => c == s)), val);
			}
		}

		[Fact]
		[Strings(
			Content = "The overload `MGen.String(int min, int max)` generates an string of length higher or equal than min and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.String(5, 7);
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.True(val.Length >= 5, string.Format("Length : {0}", val.Length));
				Assert.True(val.Length < 7, string.Format("Length : {0}", val.Length));
			}
		}

		[Fact]
		[Strings(
			Content = "The Default generator generates a string of length higher than 0 and lower than 10.",
			Order = 2)]
		public void DefaultGeneratorStringIsBetweenOneAndTen()
		{
			var generator = MGen.String();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
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
			for (int i = 0; i < 10; i++)
			{
				var value = generator.Generate().AProperty;
				Assert.NotNull(value);
				Assert.NotEqual("", value);
			}
		}

		[Fact]
		[Strings(
			Content = "Can be made to return `string?` using the `.NullableRef()` combinator.",
			Order = 4)]
		public void Nullable()
		{
			var generator = MGen.String().NullableRef();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 50; i++)
			{
				var value = generator.Generate();
				if (value == null)
				{
					isSomeTimesNull = true;
				}
				else
					isSomeTimesNotNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		public class SomeThingToGenerate
		{
			public string? AProperty { get; set; }
		}

		public class StringsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public StringsAttribute()
			{
				Caption = "Strings.";
				CaptionOrder = 2;
			}
		}
	}
}