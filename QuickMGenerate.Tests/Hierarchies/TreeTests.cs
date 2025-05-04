using QuickAcid;
using QuickAcid.Bolts;
using QuickAcid.Bolts.Nuts;
using QuickMGenerate.Tests._Tools;
using QuickMGenerate.UnderTheHood;


namespace QuickMGenerate.Tests.Hierarchies;

public class TreeTests
{
	private abstract class Tree
	{
		public abstract override string ToString();
	}

	private class Leaf : Tree
	{
		public int Value { get; set; }
		public override string ToString() => $"Leaf({Value})";
	}

	private class Branch : Tree
	{
		public Tree? Left { get; set; }
		public Tree? Right { get; set; }

		public override string ToString() => $"Node({Left}, {Right})";
	}

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
	{   // ----------------------------------------------------------------------------------
		// TODOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
		// ----------------------------------------------------------------------------------
		var generator =
			from _d in MGen.For<Tree>().Depth(1, 3) // still passes with 1, 5 need better test
			from _i in MGen.For<Tree>().GenerateAsOneOf(typeof(Branch), typeof(Leaf))
			from _l in MGen.For<Tree>().TreeLeaf<Leaf>()
			from tree in MGen.One<Tree>()
			select tree;
		new QState(
			QA.Should(generator, () => new Container<HashSet<string>>([])
				, (c, v) =>
					{
						foreach (var label in GetDepthLabels(v))
							c.Value!.Add(label);
					}
				, c =>
					!c.Value!.Contains("ERROR")
					&& c.Value!.Contains("T")
					&& c.Value!.Contains("TL")
					&& c.Value!.Contains("TR")
					&& c.Value!.Contains("TLL")
					&& c.Value!.Contains("TLR")
					&& c.Value!.Contains("TRL")
					&& c.Value!.Contains("TRR")
				, GetAssaysForTree)
		).Testify(1000);
	}

	private IEnumerable<string> GetDepthLabels(Tree tree)
	{
		if (tree == null) yield return "ERROR";

		if (tree is Leaf)
		{
			yield return "T";
			yield break;
		}

		if (tree is Branch branch)
		{
			if (branch.Left is Leaf) yield return "TL";
			if (branch.Right is Leaf) yield return "TR";

			if (branch.Left is Branch bl)
			{
				if (bl.Left is Leaf) yield return "TLL";
				if (bl.Right is Leaf) yield return "TLR";
			}
			if (branch.Right is Branch br)
			{
				if (br.Left is Leaf) yield return "TRL";
				if (br.Right is Leaf) yield return "TRR";
			}
		}
		else
		{
			yield return "ERROR";
		}
	}

	public static QAcidRunner<Acid> GetAssaysForTree(Container<HashSet<string>> c)
	{
		return
			from _1 in "Tree: Contains T".Assay(() => c.Value!.Contains("T"))
			from _2 in "Tree: Contains TL".Assay(() => c.Value!.Contains("TL"))
			from _3 in "Tree: Contains TR".Assay(() => c.Value!.Contains("TR"))
			from _4 in "Tree: Contains TLL".Assay(() => c.Value!.Contains("TLL"))
			from _5 in "Tree: Contains TLR".Assay(() => c.Value!.Contains("TLR"))
			from _6 in "Tree: Contains TRL".Assay(() => c.Value!.Contains("TRL"))
			from _7 in "Tree: Contains TRR".Assay(() => c.Value!.Contains("TRR"))
			from _ in "Tree: !Contains ERROR".Assay(() => !c.Value!.Contains("ERROR"))
			select Acid.Test;
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

	public class TreesAttribute : GeneratingHierarchiesAttribute
	{
		public TreesAttribute()
		{
			Caption = "Trees.";
			CaptionOrder = 20;
		}
	}
}