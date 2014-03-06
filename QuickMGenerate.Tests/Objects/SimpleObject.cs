using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Objects
{
	[SimpleObject(
		Content = "Use `MGen.One<T>()`, where T is the type of object you want to generate.",
		Order = 0)]
	public class SimpleObject
	{
		[Fact]
		[SimpleObject(
			Content = 
@"- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.",
			Order = 1)]
		public void FillsPrimitives()
		{
			Assert.NotEqual(0, MGen.One<SomeThingToGenerate>().Generate().AProperty);
		}

		[Fact]
		[SimpleObject(
			Content =
@"- The enumeration properties of the object will be automatically filled in using the default (or replaced) MGen.Enum<T> generator.",
			Order = 2)]
		public void FillsEnumerations()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var one = false;
			var two = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state).AnEnumeration;
				one = one || value == MyEnumeration.MyOne;
				two = two || value == MyEnumeration.Mytwo;
			}
			Assert.True(one);
			Assert.True(two);
		}

		[Fact]
		[SimpleObject(
			Content =
@"- Also works for properties with private setters.",
			Order = 1)]
		public void FillsPrivateSetterProperties()
		{
			Assert.NotEqual(0, MGen.One<SomeThingToGenerate>().Generate().APropertyWithPrivateSetters);
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var one = false;
			var two = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state).AnEnumerationWithPrivateSetter;
				one = one || value == MyEnumeration.MyOne;
				two = two || value == MyEnumeration.Mytwo;
			}
			Assert.True(one);
			Assert.True(two);
		}

		public class SomeThingToGenerate
		{
			public int AProperty { get; set; }
			public int APropertyWithPrivateSetters { get; private set; }
			public MyEnumeration AnEnumeration { get; set; }
			public MyEnumeration AnEnumerationWithPrivateSetter { get; private set; }
		}

		public enum MyEnumeration
		{
			MyOne,
			Mytwo
		}

		public class SimpleObjectAttribute : GeneratingObjectsAttribute
		{
			public SimpleObjectAttribute()
			{
				Caption = "A simple object.";
				CaptionOrder = 0;
			}
		}
	}
}