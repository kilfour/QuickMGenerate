using QuickMGenerate.Tests._Tools;
using QuickMGenerate.UnderTheHood;

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
			CheckIf.GeneratedValuesShouldEventuallySatisfyAll(generator.Select(a => a.ANullableProperty),
				("is null", a => a == null), ("is not null", a => a != null));
		}

		[Fact]
		[ReplacingPrimitiveGenerators(
			Content =
@"Replacing a nullable primitive generator does not impacts it's non-nullable counterpart.",
			Order = 2)]
		public void NullableReplace()
		{
			var generator =
				from _ in MGen.Int(666, 666).Nullable().NeverReturnNull().Replace()
				from result in MGen.One<SomeThingToGenerate>()
				select result;
			var value = generator.Generate();
			Assert.True(value.AnInt < 100, value.AnInt.ToString());
			Assert.Equal(666, value.ANullableProperty);
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
			Order = 4)]
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
			Content = "*Note :* The Replace combinator does not actually generate anything, it only influences further generation.",
			Order = 5)]
		public void ReturnsUnit()
		{
			var generator = MGen.Int(42, 42).Replace();
			Assert.Equal(Unit.Instance, generator.Generate());
		}

		[Fact]
		public void JustChecking()
		{
			var generator =
				from i in MGen.ChooseFromThese(42, 43).Unique("key")
				from result in MGen.One<SomeThingToGenerate>().Apply(s => s.AnInt = i)
				select result;

			var values = generator.Many(2).Generate().ToArray();

			Assert.NotEqual(values[0].AnInt, values[1].AnInt);
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