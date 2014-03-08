using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuickMGenerate.Tests.Hierarchies
{
	[Relations(
		Content = "In the same way one can `Customize` primitives, this can also be done for references.",
		Order = 0)]
	public class Relations
	{
		[Fact]
		[Relations(
			Content =
@"E.g. :

```
var generator =
	from product in MGen.One<ProductItem>()
	from setProduct in MGen.For<OrderLine>().Customize(orderline => orderline.Product, product)
	from orderline in MGen.One<OrderLine>()
	select orderline;
```
",
			Order = 1)]
		public void SetOneToOne()
		{
			var generator =
				from product in MGen.One<ProductItem>()
				from setProduct in MGen.For<OrderLine>().Customize(order => order.Product, product)
				from orderline in MGen.One<OrderLine>()
				select orderline;
			
			var value = generator.Generate();
			Assert.NotNull(value.Product);
		}

		[Fact]
		[Relations(
			Content =
@"In case of a one-to-many relation where the collection is inaccessible, but a method is provided for adding the many to the one,
we can use the `Apply` method, which is explained in detail in the chapter 'Other Usefull Generators'.
E.g. :

```
var generator =
	from order in MGen.One<Order>()
	from addLine in MGen.For<OrderLine>().Apply(order.AddOrderLine)
	from lines in MGen.One<OrderLine>().Many(20).ToArray()
	select order;
```
Note the `ToArray` call on the orderlines. 
This forces enumeration and is necessary because the lines are not enumerated over just by selecting the order.
",
			Order = 2)]
		public void OneToMany()
		{
			var generator =
				from order in MGen.One<Order>()
				from addLine in MGen.For<OrderLine>().Apply(order.AddOrderLine)
				from lines in MGen.One<OrderLine>().Many(2).ToArray()
				select order;

			var value = generator.Generate();
			Assert.Equal(2, value.OrderLines.Count());
		}
		[Fact]

		[Relations(
			Content =
@"If we were to select the lines instead of the order, `ToArray` would not be necessary.",
			Order = 2)]
		public void OneToManyVerifying()
		{
			var generator =
				from order in MGen.One<Order>()
				from addLine in MGen.For<OrderLine>().Apply(order.AddOrderLine)
				from lines in MGen.One<OrderLine>().Many(2)
				select lines;

			var value = generator.Generate().ToArray();
			Assert.NotNull(value[0].MyOrder);
			Assert.NotNull(value[1].MyOrder);
			Assert.Equal(value[0].MyOrder, value[1].MyOrder);
		}

		[Relations(
			Content =
@"Relations defined by constructor injection can be generated using the `One<T>(Func<T> constructor)` overload.
E.g. :

```
var generator =
	from category in MGen.One<Category>()
	from subCategory in MGen.One(() => new SubCategory(category)).Many(20)
	select category;
```
",
			Order = 3)]
		public void ThroughConstructor()
		{
			var generator =
				from category in MGen.One<Category>()
				from subCategory in MGen.One(() => new SubCategory(category)).Many(2)
				select category;

			var value = generator.Generate();
			Assert.Equal(2, value.SubCategories.Count());
		}
		
		public class Order
		{
			public Order()
			{
				OrderLines = new List<OrderLine>();
			}
			public List<OrderLine> OrderLines { get; set; }
			public void AddOrderLine(OrderLine line)
			{
				line.MyOrder = this;
				OrderLines.Add(line);
			}
		}

		public class OrderLine
		{
			public Order MyOrder { get; set; }
			public ProductItem Product { get; set; }
		}

		public class ProductItem { }

		public class Category
		{
			public Category()
			{
				SubCategories = new List<SubCategory>();
			}
			public List<SubCategory> SubCategories { get; set; }
		}

		public class SubCategory
		{
			public SubCategory(Category category)
			{
				MyCategory = category;
				category.SubCategories.Add(this);
			}
			public Category MyCategory { get; set; }
		}

		public class RelationsAttribute : GeneratingHierarchiesAttribute
		{
			public RelationsAttribute()
			{
				Caption = "Relations.";
				CaptionOrder = 1;
			}
		}
	}
}