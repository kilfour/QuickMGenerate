namespace QuickMGenerate.Tests.Objects
{
	[Inheritance(
		Content =
@"Use The `MGen.For<T>().UseThese(params Type[] types)` method chain.

F.i. :
```
MGen.For<SomeThingAbstract>().UseThese(typeof(T), typeof(SomethingDerived), typeof(SomethingElseDerived))
```",
		Order = 0)]
	public class Inheritance
	{
		[Fact]
		[Inheritance(
			Content =
@"When generating an object of type T, an object of a random chosen type from the provided list will be generated instead.",
			Order = 1)]
		public void WellItWorksDunnit()
		{
			var generator =
				from _ in MGen.For<SomeThingAbstract>().UseThese(typeof(SomeThingDerivedToGenerate))
				from thing in MGen.One<SomeThingAbstract>()
				select thing;
			var result = generator.Generate();
			Assert.IsType<SomeThingDerivedToGenerate>(result);
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