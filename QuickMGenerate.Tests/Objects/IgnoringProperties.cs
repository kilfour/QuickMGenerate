using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.Objects
{
	[IgnoringProperties(
		Content =
@"Use the `MGen.For<T>().Ignore<TProperty>(Expression<Func<T, TProperty>> func)` method chain.

F.i. :
```
MGen.For<SomeThingToGenerate>().Ignore(s => s.Id)
```",
		Order = 0)]
	public class IgnoringProperties
	{
		[Fact]
		[IgnoringProperties(
			Content = "The property specified will be ignored during generation.",
			Order = 1)]
		public void StaysDefaultValue()
		{
			var generator =
				from _ in MGen.For<SomeThingToGenerate>().Ignore(s => s.AnInt)
				from result in MGen.One<SomeThingToGenerate>()
				select result;
			Assert.Equal(0, generator.Generate().AnInt);
		}

		[Fact]
		[IgnoringProperties(
			Content = "Derived classes generated also ignore the base property.",
			Order = 2)]
		public void WorksForDerived()
		{
			var generator =
				from _ in MGen.For<SomeThingToGenerate>().Ignore(s => s.AnInt)
				from result in MGen.One<SomeThingDerivedToGenerate>()
				select result;
			Assert.Equal(0, generator.Generate().AnInt);
		}

		[Fact]
		[IgnoringProperties(
			Content =
@"Sometimes it is useful to ignore all properties while generating an object.  
For this use `MGen.For<SomeThingToGenerate>().IgnoreAll()`",
			Order = 3)]
		public void IgnoreAll()
		{
			var generator =
				from _ in MGen.For<SomeThingToGenerate>().IgnoreAll()
				from result in MGen.One<SomeThingToGenerate>()
				select result;
			Assert.Equal(0, generator.Generate().AnInt);
		}

		[Fact]
		[IgnoringProperties(
			Content =
@"`IgnoreAll()` does not ignore properties on derived classes, even inherited properties.",
			Order = 4)]
		public void IgnoreAllDerived()
		{
			var generator =
				from r in MGen.Constant(13).Replace()
				from _ in MGen.For<SomeThingToGenerate>().IgnoreAll()
				from result in MGen.One<SomeThingDerivedToGenerate>()
				select result;
			var thing = generator.Generate();
			Assert.Equal(13, thing.AnInt);
			Assert.Equal(13, thing.AnotherInt);
		}

		[Fact]
		[IgnoringProperties(
			Content = "**Note :** `The Ignore(...)` combinator does not actually generate anything, it only influences further generation.",
			Order = 4)]
		public void ReturnsUnit()
		{
			var generator = MGen.For<SomeThingToGenerate>().Ignore(s => s.AnInt);
			Assert.Equal(Unit.Instance, generator.Generate());
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
		}

		public class SomeThingDerivedToGenerate : SomeThingToGenerate
		{
			public int AnotherInt { get; set; }
		}

		public class IgnoringPropertiesAttribute : GeneratingObjectsAttribute
		{
			public IgnoringPropertiesAttribute()
			{
				Caption = "Ignoring properties.";
				CaptionOrder = 4;
			}
		}
	}
}