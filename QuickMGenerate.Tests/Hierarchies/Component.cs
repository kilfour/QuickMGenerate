using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.Hierarchies
{
	[Component(
		Content = "Use the `MGen.For<T>().Component()`, method chain.",
		Order = 0)]
	public class Component
	{
		[Fact]
		[Component(
			Content =
@"Once a component is defined, from then on it is automatically generated for any object that has a property of the components type,
similarly to how primitives are handled.",
			Order = 1)]
		public void IsGenerated()
		{
			var generator =
				from child in MGen.One<SomeChildToGenerate>()
				from thing in MGen.One<SomeThingToGenerate>().Apply(t => t.MyChild = child)
				select thing;

			var value = generator.Generate();

			Assert.NotNull(value.MyComponent);
			Assert.NotNull(value.MyChild);
			Assert.NotNull(value.MyChild.MyComponent);
		}

		[Fact]
		[Component(
			Content =
@"The only exception to the component rule is when it would lead to an infinite loop.",
			Order = 2)]
		public void AvoidsRecursion()
		{
			var generator =
				from component in MGen.For<SomeThingRecursive>().Depth(2, 2)
				from thing in MGen.One<SomeThingRecursive>()
				select thing;

			var value = generator.Generate();

			Assert.NotNull(value.Curse);
			Assert.Null(value.Curse.Curse);
		}

		[Fact]
		[Component(
			Content =
@"An overload exists which allows for controlling potentially recursive generation: `.Component(int maxDepth)`
maxDepth N means: I want up to N levels of actual structure. See `.Tree()` for a more detailed explanation 
 ",
			Order = 3)]
		public void WithDepth1()
		{
			var generator =
				from thing in MGen.One<SomeThingRecursive>()
				select thing;

			var value = generator.Generate();

			Assert.NotNull(value);
			Assert.Null(value.Curse);
		}

		[Fact]
		public void WithDepth2()
		{
			var generator =
				from component in MGen.For<SomeThingRecursive>().Depth(2, 2)
				from thing in MGen.One<SomeThingRecursive>()
				select thing;

			var value = generator.Generate();

			Assert.NotNull(value);
			Assert.NotNull(value.Curse);
			Assert.Null(value.Curse.Curse);
		}

		[Fact]
		public void WithDepth3()
		{
			var generator =
				from component in MGen.For<SomeThingRecursive>().Depth(3, 3)
				from thing in MGen.One<SomeThingRecursive>()
				select thing;

			var value = generator.Generate();

			Assert.NotNull(value);
			Assert.NotNull(value.Curse);
			Assert.NotNull(value.Curse.Curse);
			Assert.Null(value.Curse.Curse.Curse);
		}

		[Fact]
		[Component(
			Content = "*Note :* The Component 'generator' does not actually generate anything, it only influences further generation.",
			Order = 40)]
		public void ReturnsUnit()
		{
			var generator = MGen.For<SomeComponent>().Depth(1, 1);
			Assert.Equal(Unit.Instance, generator.Generate());
		}

		public class SomeThingToGenerate
		{
			public SomeComponent? MyComponent { get; set; }
			public SomeChildToGenerate? MyChild { get; set; }
			public int Fire { get; set; }
		}

		public class SomeChildToGenerate
		{
			public SomeComponent? MyComponent { get; set; }
			public int WalkWithMe { get; set; }
		}

		public class SomeComponent
		{
			public int TheAnswer { get; set; }
		}

		public class SomeThingRecursive
		{
			public SomeThingRecursive? Curse { get; set; }
		}

		public class ComponentAttribute : GeneratingHierarchiesAttribute
		{
			public ComponentAttribute()
			{
				Caption = "A 'Component'.";
				CaptionOrder = 10;
			}
		}
	}
}