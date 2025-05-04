# MGen.Tree
    just do it
--- slide --- 
# Fix Test
QuickMGenerate.Tests.Hierarchies.TreeTests.Trees()
TODO
--- slide ---

## GeneratedValuesShouldAllSatisfy

* Final version working via `CombineSpecs`
* Move `CombineSpecs` to QuickAcid core
* Used in `CheckIf.GeneratedValuesShouldAllSatisfy`

--- slide ---

## Generator Scope

* Unexpected repeated values observed during testing
* Add test cases to document behavior
* Might indicate scope or internal state reuse issue in `MGen`

> ### ⚠️ Generator Binding Scope
>
> In LINQ-style generator composition, values are **bound once per execution**, not per usage.
> This means:
>
> ```csharp
> from x in MGen.Int(1, 10)
> from _ in DoSomethingWith(x)
> from __ in DoSomethingElseWith(x)
> ```
>
> ...will use the **same `x`** throughout that run.
> If you want a new value each time, you must explicitly rebind:
>
> ```csharp
> from _ in Something()
> from x in MGen.Int(1, 10) // rebinding forces regeneration
> ```
>
> or use a `.Repeat()` combinator if available.

--- slide ---

## Recursive Structures

* Docs need update for recursive generator controls
* `from _ in MGen.Depth(min, max)` overrides the default recursion depth
* Should be added to tutorial and reference wiki

--- slide ---

## Diagnostics

* Add tests to exercise and document `Inspector` usage
* Show examples using `HasSeen`, `SeenSatisfyEach`, etc.
* Useful for teaching value tracking and runtime inspection in PBT

