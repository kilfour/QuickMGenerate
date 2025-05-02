# Generator Scope
Unexpected repeated values observed during testing.  
Add tests to document. Might need improvement.
--- slide ---
# Recursive Structures
Tree is not enough.  
(MinDepth, MaxDepth) needs to become a First Class citizen.  
Default (1, 1). Customizable through MGen.For<T>.Depth(min, max)
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

