﻿using QuickMGenerate.UnderTheHood;
using Xunit;

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
				from component in MGen.For<SomeComponent>().Component()
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
			Content = "*Note :* The Component 'generator' does not actually generate anything, it only influences further generation.",
			Order = 4)]
		public void ReturnsUnit()
		{
			var generator = MGen.For<SomeComponent>().Component();
			Assert.Equal(Unit.Instance, generator.Generate());
		}

		public class SomeThingToGenerate
		{
			public SomeComponent MyComponent { get; set; }
			public SomeChildToGenerate MyChild { get; set; }
			public int Fire { get; set; }
		}

		public class SomeChildToGenerate
		{
			public SomeComponent MyComponent { get; set; }
			public int WalkWithMe { get; set; }
		}

		public class SomeComponent
		{
			public int TheAnswer { get; set; }
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