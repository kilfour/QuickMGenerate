using QuickMGenerate.Tests._Tools;

namespace QuickMGenerate.Tests.Primitives;

[Booleans(
	Content = "Use `MGen.Bool()`. \n\nNo overload Exists.",
	Order = 0)]
public class BoolGeneration
{
	[Fact]
	[Booleans(
		Content = "The default generator generates True or False.",
		Order = 1)]
	public void DefaultGeneratorGeneratesTrueOrFalse()
	{
		CheckIf.TheseValuesAreGenerated(MGen.Bool(), true, false);
	}

	[Fact]
	[Booleans(
		Content = "Can be made to return `bool?` using the `.Nullable()` combinator.",
		Order = 2)]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(MGen.Bool().Nullable());
	}

	[Fact]
	[Booleans(
		Content = " - `bool` is automatically detected and generated for object properties.",
		Order = 3)]
	public void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			MGen.One<SomeThingToGenerate>().Select(x => x.AProperty), true, false);
	}

	[Fact]
	[Booleans(
		Content = " - `bool?` is automatically detected and generated for object properties.",
		Order = 4)]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			MGen.One<SomeThingToGenerate>().Select(x => x.ANullableProperty));
	}

	public class SomeThingToGenerate
	{
		public bool AProperty { get; set; }
		public bool? ANullableProperty { get; set; }
	}

	public class BooleansAttribute : ThePrimitiveGeneratorsAttribute
	{
		public BooleansAttribute()
		{
			Caption = "Booleans.";
			CaptionOrder = 3;
		}
	}
}