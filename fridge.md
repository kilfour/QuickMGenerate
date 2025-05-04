# Generator Scope
Unexpected repeated values observed during testing.  
Add tests to document. Might need improvement.
--- slide ---
# Recursive Structures
Update the docs !  
`from _ in MGen.Depth(min, max)` to override default  
--- slide ---
# Recursive Test Structure 
```csharp
public class MyNode { }
public class MyValue : MyNode { public object? Value; }
public class MyArray : MyNode { public List<MyNode> Values = []; }
```
--- slide ---
# Diagnostics
Add tests to document `Inspector`'s. 
--- slide ---
public abstract class Tree
{
	public abstract string ToPrettyString(string indent = "", string prefix = "");
	public override string ToString() => ToPrettyString();
}

private class Leaf : Tree
{
	public int Value { get; set; }
	public override string ToPrettyString(string indent = "", string prefix = "") =>
		$"{indent}{prefix}Leaf({Value})";
}

private class Branch : Tree
{
	public Tree? Left { get; set; }
	public Tree? Right { get; set; }

	public override string ToPrettyString(string indent = "", string prefix = "")
	{
		var nextIndent = indent + "│  ";
		return
			$"{indent}{prefix}Node\n" +
			(Left?.ToPrettyString(nextIndent, "├─ ") ?? $"{nextIndent}├─ null") + "\n" +
			(Right?.ToPrettyString(nextIndent, "└─ ") ?? $"{nextIndent}└─ null");
	}
}

Node
│  ├─ Leaf(54)
   └─ Leaf(98)
