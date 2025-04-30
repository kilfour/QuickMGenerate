using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.Objects
{
	[CustomizingProperties(
		Content =
@"Use the `MGen.For<T>().Customize<TProperty>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
MGen.For<SomeThingToGenerate>().Customize(s => s.MyProperty, MGen.Constant(42))
```",
		Order = 0)]
	public class CustomizingProperties
	{
		[Fact]
		[CustomizingProperties(
			Content = "The property specified will be generated using the passed in generator.",
			Order = 1)]
		public void StaysDefaultValue()
		{
			var generator =
				from c in MGen.For<SomeThingToGenerate>().Customize(s => s.AnInt, MGen.Constant(42))
				from r in MGen.One<SomeThingToGenerate>()
				select r;
			Assert.Equal(42, generator.Generate().AnInt);
		}

		[Fact]
		[CustomizingProperties(
			Content = "An overload exists which allows for passing a value instead of a generator.",
			Order = 2)]
		public void UsingValue()
		{
			var generator =
				from c in MGen.For<SomeThingToGenerate>().Customize(s => s.AnInt, 42)
				from r in MGen.One<SomeThingToGenerate>()
				select r;
			Assert.Equal(42, generator.Generate().AnInt);
		}

		[Fact]
		[CustomizingProperties(
			Content = "Derived classes generated also use the custom property.",
			Order = 3)]
		public void WorksForDerived()
		{
			var generator =
				from _ in MGen.For<SomeThingToGenerate>().Customize(s => s.AnInt, 42)
				from result in MGen.One<SomeThingDerivedToGenerate>()
				select result;
			Assert.Equal(42, generator.Generate().AnInt);
		}

		//[Fact(Skip="WIP")]
		//[CustomizingProperties(
		//    Content = "This does not work for fields yet.",
		//    Order = 4)]
		//public void Field()
		//{
		//    var generator =
		//        from _ in MGen.For<SomeThingToGenerate>().Customize(s => s.AnIntField, 42)
		//        from result in MGen.One<SomeThingDerivedToGenerate>()
		//        select result;
		//    Assert.Equal(42, generator.Generate().AnIntField);
		//}

		[Fact]
		[CustomizingProperties(
			Content = "*Note :* The Customize 'generator' does not actually generate anything, it only influences further generation.",
			Order = 99)]
		public void ReturnsUnit()
		{
			var generator = MGen.For<SomeThingToGenerate>().Customize(s => s.AnInt, 42);
			Assert.Equal(Unit.Instance, generator.Generate());
		}
		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
			public int AnIntField;
		}

		public class SomeThingDerivedToGenerate : SomeThingToGenerate
		{
		}

		public class CustomizingPropertiesAttribute : GeneratingObjectsAttribute
		{
			public CustomizingPropertiesAttribute()
			{
				Caption = "Customizing properties.";
				CaptionOrder = 5;
			}
		}
	}
}