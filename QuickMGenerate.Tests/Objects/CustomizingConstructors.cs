using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.Objects
{
	[CustomizingConstructors(
		Content =
@"Use the `MGen.For<T>().Construct<TArg>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
MGen.For<SomeThing>().Construct(MGen.Constant(42))
```",
		Order = 0)]
	public class CustomizingConstructors
	{
		[Fact]
		[CustomizingConstructors(
			Content = "Subsequent calls to `MGen.One<T>()` will then use the registered constructor.",
			Order = 1)]
		public void Works()
		{
			var generator =
				from i in MGen.For<SomeThing>().Ignore(a => a.AnInt1)
				from c in MGen.For<SomeThing>().Construct(MGen.Constant(42))
				from r in MGen.One<SomeThing>()
				select r;
			Assert.Equal(42, generator.Generate().AnInt1);
		}

		[Fact]
		[CustomizingConstructors(
			Content =
@"Various overloads exist : 
 -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2)`",
			Order = 2)]
		public void WorksTwoArgs()
		{
			var generator =
				from i1 in MGen.For<SomeThing>().Ignore(a => a.AnInt1)
				from i2 in MGen.For<SomeThing>().Ignore(a => a.AnInt2)
				from c in MGen.For<SomeThing>().Construct(MGen.Constant(42), MGen.Constant(43))
				from r in MGen.One<SomeThing>()
				select r;
			var result = generator.Generate();
			Assert.Equal(42, result.AnInt1);
			Assert.Equal(43, result.AnInt2);
		}

		[Fact]
		[CustomizingConstructors(
			Content = " -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3)`",
			Order = 3)]
		public void WorksThreeArgs()
		{
			var generator =
				from i1 in MGen.For<SomeThing>().Ignore(a => a.AnInt1)
				from i2 in MGen.For<SomeThing>().Ignore(a => a.AnInt2)
				from i3 in MGen.For<SomeThing>().Ignore(a => a.AnInt3)
				from c in MGen.For<SomeThing>().Construct(MGen.Constant(42), MGen.Constant(43), MGen.Constant(44))
				from r in MGen.One<SomeThing>()
				select r;
			var result = generator.Generate();
			Assert.Equal(42, result.AnInt1);
			Assert.Equal(43, result.AnInt2);
			Assert.Equal(44, result.AnInt3);
		}

		[Fact]
		[CustomizingConstructors(
			Content = " -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4)`",
			Order = 4)]
		public void WorksFourArgs()
		{
			var generator =
				from i1 in MGen.For<SomeThing>().Ignore(a => a.AnInt1)
				from i2 in MGen.For<SomeThing>().Ignore(a => a.AnInt2)
				from i3 in MGen.For<SomeThing>().Ignore(a => a.AnInt3)
				from i4 in MGen.For<SomeThing>().Ignore(a => a.AnInt4)
				from c in MGen.For<SomeThing>().Construct(MGen.Constant(42), MGen.Constant(43), MGen.Constant(44), MGen.Constant(45))
				from r in MGen.One<SomeThing>()
				select r;
			var result = generator.Generate();
			Assert.Equal(42, result.AnInt1);
			Assert.Equal(43, result.AnInt2);
			Assert.Equal(44, result.AnInt3);
			Assert.Equal(45, result.AnInt4);
		}

		[Fact]
		[CustomizingConstructors(
			Content =
@" -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4, Generator<T5> g5)`  

After that, ... you're on your own.",
			Order = 5)]
		public void WorksFiveArgs()
		{
			var generator =
				from i1 in MGen.For<SomeThing>().Ignore(a => a.AnInt1)
				from i2 in MGen.For<SomeThing>().Ignore(a => a.AnInt2)
				from i3 in MGen.For<SomeThing>().Ignore(a => a.AnInt3)
				from i4 in MGen.For<SomeThing>().Ignore(a => a.AnInt4)
				from i5 in MGen.For<SomeThing>().Ignore(a => a.AString)
				from c in MGen.For<SomeThing>().Construct(MGen.Constant(42), MGen.Constant(43), MGen.Constant(44), MGen.Constant(45), MGen.Constant("answer"))
				from r in MGen.One<SomeThing>()
				select r;
			var result = generator.Generate();
			Assert.Equal(42, result.AnInt1);
			Assert.Equal(43, result.AnInt2);
			Assert.Equal(44, result.AnInt3);
			Assert.Equal(45, result.AnInt4);
			Assert.Equal("answer", result.AString);
		}

		[Fact]
		[CustomizingConstructors(
			Content =
@"Or use the factory method overload:  
`MGen.For<T>().Construct<T>(Func<T> ctor)`",
			Order = 6)]
		public void FactoryCtor()
		{
			var generator =
				from i1 in MGen.For<SomeThing>().Ignore(a => a.AnInt1)
				from i2 in MGen.For<SomeThing>().Ignore(a => a.AnInt2)
				from i3 in MGen.For<SomeThing>().Ignore(a => a.AnInt3)
				from i4 in MGen.For<SomeThing>().Ignore(a => a.AnInt4)
				from i5 in MGen.For<SomeThing>().Ignore(a => a.AString)
				from c in MGen.For<SomeThing>().Construct(() => new SomeThing(1, 2, 3, 4, "5"))
				from r in MGen.One<SomeThing>()
				select r;
			var result = generator.Generate();
			Assert.Equal(1, result.AnInt1);
			Assert.Equal(2, result.AnInt2);
			Assert.Equal(3, result.AnInt3);
			Assert.Equal(4, result.AnInt4);
			Assert.Equal("5", result.AString);
		}

		// throws InvalidOperationException

		[Fact]
		[CustomizingConstructors(
			Content = "*Note :* The Construct 'generator' does not actually generate anything, it only influences further generation.",
			Order = 99)]
		public void ReturnsUnit()
		{
			var generator = MGen.For<SomeThing>().Customize(s => s.AnInt1, 42);
			Assert.Equal(Unit.Instance, generator.Generate());
		}

		public class SomeThing
		{
			public int? AnInt1 { get; private set; }
			public int? AnInt2 { get; private set; }
			public int? AnInt3 { get; private set; }
			public int? AnInt4 { get; private set; }
			public string? AString { get; private set; }

			public SomeThing(int anInt1)
			{
				AnInt1 = anInt1;
			}

			public SomeThing(int anInt1, int anInt2)
			{
				AnInt1 = anInt1;
				AnInt2 = anInt2;
			}

			public SomeThing(int anInt1, int anInt2, int anInt3)
			{
				AnInt1 = anInt1;
				AnInt2 = anInt2;
				AnInt3 = anInt3;
			}

			public SomeThing(int anInt1, int anInt2, int anInt3, int anInt4)
			{
				AnInt1 = anInt1;
				AnInt2 = anInt2;
				AnInt3 = anInt3;
				AnInt4 = anInt4;
			}

			public SomeThing(int anInt1, int anInt2, int anInt3, int anInt4, string aString)
			{
				AnInt1 = anInt1;
				AnInt2 = anInt2;
				AnInt3 = anInt3;
				AnInt4 = anInt4;
				AString = aString;
			}
		}


		public class CustomizingConstructorsAttribute : GeneratingObjectsAttribute
		{
			public CustomizingConstructorsAttribute()
			{
				Caption = "Customizing constructors.";
				CaptionOrder = 6;
			}
		}
	}
}