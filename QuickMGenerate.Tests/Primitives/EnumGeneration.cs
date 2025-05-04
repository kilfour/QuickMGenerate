using QuickMGenerate.Tests._Tools;

namespace QuickMGenerate.Tests.Primitives
{
	[Enums(
		Content = "Use `MGen.Enum<T>()`, where T is the type of Enum you want to generate. \n\nNo overload Exists.",
		Order = 0)]
	public class EnumGeneration
	{
		[Fact]
		[Enums(
			Content =
"The default generator just picks a random value from all enemeration values.",
			Order = 1)]
		public void DefaultGenerator()
		{
			CheckIf.TheseValuesAreGenerated(MGen.Enum<MyEnumeration>(),
				MyEnumeration.MyOne, MyEnumeration.Mytwo);
		}

		[Fact]
		[Enums(
			Content = " - An Enumeration is automatically detected and generated for object properties.",
			Order = 2)]
		public void Property()
		{
			CheckIf.TheseValuesAreGenerated(
				MGen.One<SomeThingToGenerate>().Select(a => a.AnEnumeration),
				MyEnumeration.MyOne, MyEnumeration.Mytwo);
		}

		[Fact]
		[Enums(
			Content = " - A nullable Enumeration is automatically detected and generated for object properties.",
			Order = 3)]
		public void NullableProperty()
		{
			CheckIf.GeneratesNullAndNotNull(
				MGen.One<SomeThingToGenerate>().Select(a => a.ANullableProperty));
		}

		[Fact]
		[Enums(
			Content = " - Passing in a non Enum type for T throws an ArgumentException.",
			Order = 3)]
		public void Throws()
		{
			Assert.Throws<ArgumentException>(() => MGen.Enum<int>().Generate());
		}

		public class SomeThingToGenerate
		{
			public MyEnumeration AnEnumeration { get; set; }
			public MyEnumeration? ANullableProperty { get; set; }
		}

		public enum MyEnumeration { MyOne, Mytwo }

		public class EnumsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public EnumsAttribute()
			{
				Caption = "Enums.";
				CaptionOrder = 20;
			}
		}
	}
}