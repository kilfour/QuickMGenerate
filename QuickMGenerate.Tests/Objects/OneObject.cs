using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Objects
{
	[OneObject(
		Content = "Use `MGen.One<T>()`, where T is the type of object you want to generate.",
		Order = 0)]
	public class OneObject
	{
		[Fact]
		[OneObject(
			Content = 
@"- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.",
			Order = 1)]
		public void FillsPrimitives()
		{
			Assert.NotEqual(0, MGen.One<SomeThingToGenerate>().Generate().AProperty);
		}

		[Fact]
		[OneObject(
			Content =
@"- The enumeration properties of the object will be automatically filled in using the default (or replaced) MGen.Enum<T> generator.",
			Order = 2)]
		public void FillsEnumerations()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var one = false;
			var two = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate().AnEnumeration;
				one = one || value == MyEnumeration.MyOne;
				two = two || value == MyEnumeration.Mytwo;
			}
			Assert.True(one);
			Assert.True(two);
		}

		[Fact]
		[OneObject(
			Content =
@"- Also works for properties with private setters.",
			Order = 3)]
		public void FillsPrivateSetterProperties()
		{
			Assert.NotEqual(0, MGen.One<SomeThingToGenerate>().Generate().APropertyWithPrivateSetters);
			var generator = MGen.One<SomeThingToGenerate>();
			var one = false;
			var two = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate().AnEnumerationWithPrivateSetter;
				one = one || value == MyEnumeration.MyOne;
				two = two || value == MyEnumeration.Mytwo;
			}
			Assert.True(one);
			Assert.True(two);
		}

//	    [Fact(Skip="WIP")]
//	    [OneObject(
//	        Content =
//@"- Also works for public fields (Not Yet).",
//	        Order = 4)]
//	    public void FillsPublicFields()
//	    {
//	        Assert.NotEqual(0, MGen.One<SomeThingToGenerate>().Generate().APublicField);
//	    }

        [Fact]
		[OneObject(
			Content =
@"- Can generate any object that has a parameterless constructor, be it public, protected, or private.",
			Order = 10)]
		public void CanGenerateObjectsProtectedAndPrivate()
		{
			MGen.One<SomeThingProtectedToGenerate>().Generate();
			MGen.One<SomeThingPrivateToGenerate>().Generate();
		}

		[Fact]
		[OneObject(
			Content =
@"- The overload `MGen.One<T>(Func<T> constructor)` allows for specific constructor selection.",
			Order = 11)]
		public void CustomConstructor()
		{
			var generator =
				from ignore in MGen.For<SomeThingWithAnAnswer>().Ignore(e => e.Answer)
				from result in MGen.One(() => new SomeThingWithAnAnswer(42))
				select result;
			Assert.Equal(42, generator.Generate().Answer);
		}

		public class SomeThingToGenerate
		{
			public int AProperty { get; set; }
			public int APropertyWithPrivateSetters { get; private set; }
			public MyEnumeration AnEnumeration { get; set; }
			public MyEnumeration AnEnumerationWithPrivateSetter { get; private set; }

		    public int APublicField;
		}

		public enum MyEnumeration
		{
			MyOne,
			Mytwo
		}

		public class SomeThingProtectedToGenerate
		{
			protected SomeThingProtectedToGenerate(){ }
		}

		public class SomeThingPrivateToGenerate
		{
			protected SomeThingPrivateToGenerate() { }
		}

		public class SomeThingWithAnAnswer
		{
			public int Answer { get; set; }

			public SomeThingWithAnAnswer(int answer)
			{
				Answer = answer;
			}
		}

		public class OneObjectAttribute : GeneratingObjectsAttribute
		{
			public OneObjectAttribute()
			{
				Caption = "A simple object.";
				CaptionOrder = 0;
			}
		}
	}
}