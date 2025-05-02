# Generator Scope
Add tests to document.  
Might need improvement.
--- slide ---
# Recursive Structures
Tree is not enough.
```csharp
public class MyNode { }
public class MyValue : MyNode { public object? Value; }
public class MyArray : MyNode { public List<MyNode> Values = []; }
```

