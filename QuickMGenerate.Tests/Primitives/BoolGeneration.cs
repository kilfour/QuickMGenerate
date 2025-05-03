using QuickAcid;
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
	public void DefaultGeneratorSometimesGeneratesTrue()
	{
		new QState(QA.ShouldEventuallyBe("Bool is sometimes true", MGen.Bool(), (c, v) => c.Value |= v, c => c.Value))
			.Testify(100);
	}

	[Fact]
	[Booleans(
		Content = "Can be made to return `bool?` using the `.Nullable()` combinator.",
		Order = 2)]
	public void Nullable()
	{
		var generator = MGen.Bool().Nullable();
		new QState(QA.ShouldEventuallyBeNullAndNotNull("Bool", generator)).Testify(100);
	}

	[Fact]
	[Booleans(
		Content = " - `bool` is automatically detected and generated for object properties.",
		Order = 3)]
	public void Property()
	{
		var generator = MGen.One<SomeThingToGenerate>();
		var isTrue = false;
		for (int i = 0; i < 10; i++)
		{
			isTrue = isTrue || generator.Generate().AProperty;
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
		var isSomeTimesNull = false;
		var isSomeTimesNotNull = false;
		for (int i = 0; i < 20; i++)
		{
			var value = generator.Generate().ANullableProperty;
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

	public class BooleansAttribute : ThePrimitiveGeneratorsAttribute
	{
		public BooleansAttribute()
		{
			Caption = "Booleans.";
			CaptionOrder = 3;
		}
	}
}