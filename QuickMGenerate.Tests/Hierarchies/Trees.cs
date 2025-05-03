namespace QuickMGenerate.Tests.Hierarchies;

[Trees(
	Content =
@"Trees are a special kind of hierarchy and care needs to be taken to avoid recursion and such.  

They get their own generator : `MGen.Tree<TBase, TEnd>(int maxDepth, params Type[] derivedTypes)`  
Params explained :
 - `TBase` : the base type of your tree structure.  
 - `TEnd` : the *leaf* type of your tree structure.  
 - `derivedTypes` : all types that exist in your tree hierarchy, make sure to also include TEnd.",
	Order = 0)]
public class Trees
{
	[Fact(Skip = "obsolete")]
	[Trees(
		Content =
@"And then there's maxDepth ...  
maxDepth N means: I want up to N levels of actual structure.  
Once I hit depth N, I must return a Leaf.  
That is:
 - maxDepth = 1 → just a Leaf
 - maxDepth = 2 → a Node(Leaf, Leaf)
 - maxDepth = 3 → a Node(Node(Leaf, Leaf), Leaf)
")]
	public void CheckLeafDepth1()
	{
		for (int i = 0; i < 100; i++)
		{
			var generator = MGen.Tree<Tree, Leaf>(1, typeof(Leaf), typeof(Node));

			var value = generator.Generate();

			Assert.NotNull(value);
			Assert.IsType<Leaf>(value);
		}
	}

	[Fact(Skip = "obsolete")]
	public void CheckLeafDepth2()
	{
		for (int i = 0; i < 100; i++)
		{
			var generator = MGen.Tree<Tree, Leaf>(2, typeof(Leaf), typeof(Node));

			var value = generator.Generate();

			Assert.NotNull(value);
			if (value is Node node)
			{
				Assert.IsType<Leaf>(node.Left);
				Assert.IsType<Leaf>(node.Right);
			}
		}
	}

	[Fact(Skip = "obsolete")]
	public void CheckLeafDepth3()
	{
		for (int i = 0; i < 100; i++)
		{
			var generator = MGen.Tree<Tree, Leaf>(3, typeof(Leaf), typeof(Node));

			var value = generator.Generate();

			Assert.NotNull(value);
			if (value is Node node && node.Left is Node nextNode)
			{
				Assert.IsType<Leaf>(nextNode.Left);
				Assert.IsType<Leaf>(nextNode.Right);
			}
		}
	}

	[Fact(Skip = "obsolete")]
	[Trees(
		Content = "Throws an ArgumentException if maxDepth < 1.",
		Order = 3)]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => MGen.Tree<Tree, Leaf>(0, typeof(Leaf), typeof(Node)));
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

	record Node : Tree
	{
		public Tree? Left { get; set; }
		public Tree? Right { get; set; }
		public Node() { } // required for component-based construction
		public Node(Tree left, Tree right) => (Left, Right) = (left, right);

		public override string ToString() => $"Node({Left}, {Right})";
	}

	public class TreesAttribute : GeneratingHierarchiesAttribute
	{
		public TreesAttribute()
		{
			Caption = "Generating Trees.";
			CaptionOrder = 11;
		}
	}
}