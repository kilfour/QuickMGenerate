Add a .NonNull() extension to kill nullability from .Nullable() types easily.

Add a .Shuffle() combinator on collections to make things even more chaotic.

Add an optional "Seed" to force deterministic outputs when wanted (great for debugging).

Fuzz() as a higher-level abstraction for randomizing small object graphs automatically.

ctor :
MGen.For<Leaf>().Construct(MGen.Int())
MGen.For<Node>().Construct(MGen.One<Tree>(), MGen.One<Tree>())