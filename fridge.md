# Generator Scope
Unexpected repeated values observed during testing.  
Add tests to document. Might need improvement.
--- slide ---
# Recursive Structures
Update the docs !  
`from _ in MGen.Depth(min, max)` to override default  
`// todo choose(null, ctor)` in `DepthControlledCreation`
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

