using Xunit;

namespace QuickMGenerate.Tests.Objects
{
	[ReplacingPrimitiveGenerators(Content = @"Use the `.Replace()` extension method.", Order = 0)]
	public class ReplacingPrimitiveGenerators
	{
		[Fact]
		[ReplacingPrimitiveGenerators(
			Content =
@"Example
```
var generator =
	from _ in MGen.Constant(42).Replace()
	from result in MGen.One<SomeThingToGenerate>()
	select result;
```
When executing above generator it will return a SomeThingToGenerate object where all integers have the value 42.

Keep in mind that the .Replace() call returns a 'Unit' generator. 
I.e. it does not really generate anything on it's own.
", 
			Order = 1)]
		public void UsesReplacement()
		{
			var generator =
				from _ in MGen.Constant(42).Replace()
				from result in MGen.One<SomeThingToGenerate>()
				select result;

			var value = generator.Generate();

			Assert.Equal(42, value.AnInt);
		}

		[Fact]
		public void NullableUsesReplacement()
		{
			var generator =
				from _ in MGen.Int(42, 42).Nullable().NeverReturnNull().Replace()
				from result in MGen.One<SomeThingToGenerate>()
				select result;

			var value = generator.Generate();

			Assert.Equal(42, value.ANullableProperty);
		}

		[Fact]
		[ReplacingPrimitiveGenerators(
			Content =
@"Replacements can occur multiple times during one generation :
```
var generator =
	from _ in MGen.Constant(42).Replace()
	from result1 in MGen.One<SomeThingToGenerate>()
	from __ in MGen.Constant(666).Replace()
	from result2 in MGen.One<SomeThingToGenerate>()
	select new[] { result1, result2 };
```
When executing above generator result1 will have all integers set to 42 and result2 to 666.",
			Order = 3)]
		public void MultipleReplacements()
		{
			var generator =
				from _ in MGen.Constant(42).Replace()
				from result1 in MGen.One<SomeThingToGenerate>()
				from __ in MGen.Constant(666).Replace()
				from result2 in MGen.One<SomeThingToGenerate>()
				select new[] { result1, result2 };

			var array = generator.Generate();

			Assert.Equal(42, array[0].AnInt);
			Assert.Equal(666, array[1].AnInt);
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
			public int? ANullableProperty { get; set; }
		}

		public class ReplacingPrimitiveGeneratorsAttribute : GeneratingObjectsAttribute
		{
			public ReplacingPrimitiveGeneratorsAttribute()
			{
				Caption = "Replacing Primitive Generators";
				CaptionOrder = 30;
			}
		}
	}
}