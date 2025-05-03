using QuickAcid;
using QuickAcid.Bolts;
using QuickAcid.Bolts.Nuts;
using QuickMGenerate.Tests._Tools;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.Hierarchies;

public class Simple { public Simple Child { get; set; } }
public class Recurse
{
	public Recurse? Child { get; set; }
	public NoRecurse? OtherChild { get; set; }
	public override string ToString()
	{
		var childString =
			Child == null ? "<null>" : Child.ToString();
		var otherChildString =
			OtherChild == null ? "<null>" : "{ NoRecurse }";
		return $"{{ Recurse: Child = {childString}, OtherChild = {otherChildString} }}";
	}
}

public class NoRecurse { }

[DepthControl(
	Content =
@"As mentioned in the *A simple object section*: “The object properties will also be automatically filled in.”
However, this automatic population only applies to the first level of object properties.
Deeper properties will remain null unless configured otherwise.  
So if we have the following class :
```csharp
public class NoRecurse { }
public class Recurse
{
	public Recurse Child { get; set; }
	public NoRecurse OtherChild { get; set; }
	public override string ToString()
	{
		var childString =
			Child == null ? ""<null>"" : Child.ToString();
		var otherChildString =
			OtherChild == null ? ""<null>"" : ""{ NoRecurse }"";
		return $""{{ Recurse: Child = {childString}, OtherChild = {otherChildString} }}"";
	}
}
```",
	Order = 0)]
public class DepthControlTests
{
	[Fact]
	[DepthControl(
		Content =
@"If we then do :
```csharp
Console.WriteLine(MGen.One<Recurse>().Generate().ToString());
```
It outputs : 
```
{ Recurse: Child = <null>, OtherChild = { NoRecurse } }
```
While this may seem counter-intuitive, it is an intentional default to prevent infinite recursion or overly deep object trees.
Internally, a `DepthConstraint(int Min, int Max)` is registered per type.
The default values are `new(1, 1)`.  
Revisiting our example we can see that both types have indeed been generated with these default values.",
		Order = 1)]
	public void DefaultDepth()
	{
		var generator = MGen.One<Recurse>();

		var value = generator.Generate();

		Assert.NotNull(value);
		Assert.Null(value.Child);
		Assert.NotNull(value.OtherChild);
	}

	[Fact]
	[DepthControl(
		Content =
@"You can control generation depth per type using the `.Depth(min, max)` combinator.  
For instance:
```csharp
var generator =
	from _ in MGen.For<Recurse>().Depth(2, 2)
	from recurse in MGen.One<Recurse>()
	select recurse;
Console.WriteLine(generator.Generate().ToString());
```
Outputs:
```
{ Recurse: Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } }
, OtherChild = { NoRecurse } 
}
```
 ",
		Order = 2)]
	public void WithDepth2()
	{
		var generator =
			from _ in MGen.For<Recurse>().Depth(2, 2)
			from recurse in MGen.One<Recurse>()
			select recurse;

		var value = generator.Generate();

		Assert.NotNull(value);
		Assert.NotNull(value.OtherChild);
		Assert.NotNull(value.Child);
		Assert.NotNull(value.Child.OtherChild);
		Assert.Null(value.Child.Child);
	}

	[Fact]
	[DepthControl(
		Content =
@"Recap:
```
Depth(1, 1)
{ Recurse: Child = <null>, OtherChild = { NoRecurse } }

Depth(2, 2)
{ Recurse: 
	Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } },
  	OtherChild = { NoRecurse } 
}

Depth(3, 3)
{ Recurse: 
	Child = { Recurse: 
		Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } },
        OtherChild = { NoRecurse } },
  	OtherChild = { NoRecurse } 
}
```
 ",
		Order = 3)]
	public void WithDepth3()
	{
		var generator =
			from _ in MGen.For<Recurse>().Depth(3, 3)
			from thing in MGen.One<Recurse>()
			select thing;

		var value = generator.Generate();

		Assert.NotNull(value);
		Assert.NotNull(value.OtherChild);
		Assert.NotNull(value.Child);
		Assert.NotNull(value.Child.OtherChild);
		Assert.NotNull(value.Child.Child);
		Assert.NotNull(value.Child.Child.OtherChild);
		Assert.Null(value.Child.Child.Child);
	}

	[Fact]
	[DepthControl(
		Content =
@"Using for instance `.Depth(1, 3)` allows the generator to randomly choose a depth between 1 and 3 (inclusive) for that type.
This means some instances will be shallow, while others may be more deeply nested, introducing variability within the defined bounds.",
		Order = 4)]
	public void WithDepth_1_5()
	{
		var generator =
			from _ in MGen.For<Recurse>().Depth(1, 3)
			from thing in MGen.One<Recurse>()
			select thing;

		new QState(
			QA.Should(generator, () => new Container<HashSet<string>>([])
				, (c, v) => c.Value!.Add(GetDepthString(v))
				, c => !c.Value!.Contains("4")
					&& c.Value!.Contains("1")
					&& c.Value!.Contains("2")
					&& c.Value!.Contains("3")
				, GetAssays)
		).Testify(1000);
	}

	public static QAcidRunner<Acid> GetAssays(Container<HashSet<string>> c)
	{
		return
			  from _1 in "DepthControl: Contains 1".Assay(() => c.Value!.Contains("1"))
			  from _2 in "DepthControl: Contains 2".Assay(() => c.Value!.Contains("2"))
			  from _3 in "DepthControl: Contains 3".Assay(() => c.Value!.Contains("3"))
			  from _6 in "DepthControl: !Contains 4".Assay(() => !c.Value!.Contains("4"))
			  select Acid.Test;
	}

	public string GetDepthString(Recurse v)
	{
		if (v == null)
			throw new Exception("BOOOOOOOOOOOOOOOM");
		if (v.Child == null) return "1";
		if (v.Child.Child == null) return "2";
		if (v.Child.Child.Child == null) return "3";
		return "4";
	}

	abstract record Tree
	{
		public abstract override string ToString();
	}

	record Leaf : Tree
	{
		public int Value { get; set; }
		public Leaf() { } // required for component-based construction
		public Leaf(int value) => Value = value;

		public override string ToString() => $"Leaf({Value})";
	}

	record Branch : Tree
	{
		public Tree? Left { get; set; }
		public Tree? Right { get; set; }
		public Branch() { } // required for component-based construction
		public Branch(Tree left, Tree right) => (Left, Right) = (left, right);

		public override string ToString() => $"Node({Left}, {Right})";
	}

	[Fact]
	[DepthControl(
		Content =
@"Depth control together with the `.GenerateAsOneOf(...)` combinator mentioned above and the previously unmentioned `.TreeLeaf<T>()` one allows you to build tree type hierarchies.  
Given the cannonical abstract Tree, concrete Branch and Leaf example model, we can generate this like so:
```csharp
```",
		Order = 4)]
	public void Trees()
	{
		var generator =
			from _d in MGen.For<Tree>().Depth(1, 2)
			from _i in MGen.For<Tree>().GenerateAsOneOf(typeof(Branch), typeof(Leaf))
			from _l in MGen.For<Tree>().TreeLeaf<Leaf>()
			from thing in MGen.One<Tree>()
			select thing;

		new QState(
			QA.Should(generator, () => new Container<HashSet<string>>([])
				, (c, v) => c.Value!.Add(GetDepthString(v))
				, c => !c.Value!.Contains("4")
					&& c.Value!.Contains("1")
					&& c.Value!.Contains("2")
					&& c.Value!.Contains("3")
				, GetAssays)
		).Testify(1000);
	}

	private string GetDepthString(Tree tree)
	{
		if (tree == null)
			throw new Exception("BOOOOOOOOOOOOOOOM");
		var branch = (Branch)tree;
		if (branch == null) return "T";
		if (branch.Left == null) throw new Exception("branch.Left == null");
		if (branch.Right == null) throw new Exception("branch.Right == null");
		return "ERROR";
	}

	public static QAcidRunner<Acid> GetAssaysForTree(Container<HashSet<string>> c)
	{
		return
			  from _1 in "Tree: Contains T".Assay(() => c.Value!.Contains("T"))
				  //   from _2 in "DepthControl: Contains 2".Assay(() => c.Value!.Contains("2"))
				  //   from _3 in "DepthControl: Contains 3".Assay(() => c.Value!.Contains("3"))
				  //   from _6 in "DepthControl: !Contains 4".Assay(() => !c.Value!.Contains("4"))
			  select Acid.Test;
	}

	[Fact]
	[DepthControl(
		Content = "**Note :** The `Depth(...)` combinator does not actually generate anything, it only influences further generation.",
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

	public class DepthControlAttribute : GeneratingHierarchiesAttribute
	{
		public DepthControlAttribute()
		{
			Caption = "Depth Control.";
			CaptionOrder = 10;
		}
	}
}