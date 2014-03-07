using QuickMGenerate.UnderTheHood;
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
		[ReplacingPrimitiveGenerators(
			Content =
@"Replacing a primitive generator automatically impacts it's nullable counterpart.",
			Order = 2)]
		public void NullableUsesReplacement()
		{
			var generator =
				from _ in MGen.Int(42, 42).Replace()
				from result in MGen.One<SomeThingToGenerate>()
				select result;

			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 30; i++)
			{
				var value = generator.Generate(state).ANullableProperty;
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.Equal(42, value.Value);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
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

		[Fact]
		[ReplacingPrimitiveGeneratorsAttribute(
			Content = "*Note :* The Replace 'generator' does not actually generate anything, it only influences further generation.",
			Order = 4)]
		public void ReturnsUnit()
		{
			var generator = MGen.Int(42,42).Replace();
			Assert.Equal(Unit.Instance, generator.Generate());
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