using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.Objects
{
	[Inheritance(
		Content =
@"Use The `MGen.For<T>().GenerateAsOneOf(params Type[] types)` method chain.

F.i. :
```
MGen.For<SomeThingAbstract>().GenerateAsOneOf(
	typeof(SomethingDerived), typeof(SomethingElseDerived))
```",
		Order = 0)]
	public class Inheritance
	{
		[Fact]
		[Inheritance(
			Content =
@"When generating an object of type T, an object of a random chosen type from the provided list will be generated instead.",
			Order = 1)]
		public void UsingDerived()
		{
			var generator =
				from _ in MGen.For<SomeThingAbstract>().GenerateAsOneOf(typeof(SomeThingDerivedToGenerate))
				from thing in MGen.One<SomeThingAbstract>()
				select thing;
			var result = generator.Generate();
			Assert.IsType<SomeThingDerivedToGenerate>(result);
		}

		[Fact]
		[Inheritance(
			Content = "**Note :** The `GenerateAsOneOf(...)` combinator does not actually generate anything, it only influences further generation.",
			Order = 4)]
		public void ReturnsUnit()
		{
			var generator = MGen.For<SomeThingAbstract>().GenerateAsOneOf(typeof(SomeThingDerivedToGenerate));
			Assert.Equal(Unit.Instance, generator.Generate());
		}

		public abstract class SomeThingAbstract
		{
			public int AnInt { get; set; }
		}

		public class SomeThingDerivedToGenerate : SomeThingAbstract
		{
		}

		public class InheritanceAttribute : GeneratingObjectsAttribute
		{
			public InheritanceAttribute()
			{
				Caption = "Inheritance.";
				CaptionOrder = 11;
			}
		}
	}
}