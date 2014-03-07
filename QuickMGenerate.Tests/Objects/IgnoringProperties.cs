using QuickMGenerate.UnderTheHood;
using Xunit;

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
			Content = "*Note :* The Ignore 'generator' does not actually generate anything, it only influences further generation.",
			Order = 3)]
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