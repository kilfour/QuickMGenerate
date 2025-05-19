using QuickAcid.Bolts;
using QuickMGenerate;
using QuickMGenerate.Tests._Tools;
using QuickMGenerate.UnderTheHood;


namespace QuickMGenerate.Tests.Hierarchies;

public class TreeTests
{
	[Fact]
	[Trees(
		Content =
@"Depth control together with the `.GenerateAsOneOf(...)` combinator mentioned above and the previously unmentioned `TreeLeaf<T>()` one allows you to build tree type hierarchies.  
Given the cannonical abstract Tree, concrete Branch and Leaf example model, we can generate this like so:
```csharp
var generator =
	from _d in MGen.For<Tree>().Depth(1, 3)
	from _i in MGen.For<Tree>().GenerateAsOneOf(typeof(Branch), typeof(Leaf))
	from _l in MGen.For<Tree>().TreeLeaf<Leaf>()
	from tree in MGen.One<Tree>()
	select tree;
```
Our leaf has an int value property, so the following:
```csharp
Console.WriteLine(generator.Generate().ToString());
```	
Would output something like:
```
Node(Leaf(31), Node(Leaf(71), Leaf(10)))
```
",
		Order = 1)]
	public void Trees()
	{
		var generator =
			from _ in MGen.For<Tree>().Depth(1, 3)
			from __ in MGen.For<Tree>().GenerateAsOneOf(typeof(Branch), typeof(Leaf))
			from ___ in MGen.For<Tree>().TreeLeaf<Leaf>()
			from tree in MGen.One<Tree>()
			select tree.ToLabel();

		var validLabels = new[] { "E", "LE", "RE", "LLE", "LRE", "RLE", "RRE" };

		CheckIf.GeneratedValuesShouldEventuallySatisfyAll(100,
			generator,
			("has E", s => s.Split("|").Contains("E")),
			("has LE", s => s.Split("|").Contains("LE")),
			("has RE", s => s.Split("|").Contains("RE")),
			("has LLE", s => s.Split("|").Contains("LLE")),
			("has LRE", s => s.Split("|").Contains("LRE")),
			("has RLE", s => s.Split("|").Contains("RLE")),
			("has RRE", s => s.Split("|").Contains("RRE")),
			("valid", s => s.Split("|").All(validLabels.Contains))
		);
	}

	[Fact]
	[Trees(
		Content = "**Note :** The `TreeLeaf<T>()` combinator does not actually generate anything, it only influences further generation.",
		Order = 40)]
	public void ReturnsUnit()
	{
		var generator = MGen.For<Tree>().Depth(1, 1);
		Assert.Equal(Unit.Instance, generator.Generate());
	}

	private abstract class Tree
	{
		public abstract string ToLabel();
	}

	private class Leaf : Tree
	{
		public int Value { get; set; }
		public override string ToString() => "Leaf";

		public override string ToLabel() => "E";
	}

	private class Branch : Tree
	{
		public Tree? Left { get; set; }
		public Tree? Right { get; set; }

		public override string ToString() => $"Node";
		public override string ToLabel()
		{
			return string.Join("|",
				Prefix("L", Left!.ToLabel()),
				Prefix("R", Right!.ToLabel()));
		}

		private string Prefix(string prefix, string labels)
		{
			return string.Join("|",
				labels.Split('|').Select(label => prefix + label));
		}
	}

	public class TreesAttribute : GeneratingHierarchiesAttribute
	{
		public TreesAttribute()
		{
			Caption = "Trees.";
			CaptionOrder = 20;
		}
	}
}