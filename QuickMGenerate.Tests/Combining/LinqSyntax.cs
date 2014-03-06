using QuickMGenerate.Tests.Objects;
using Xunit;

namespace QuickMGenerate.Tests.Combining
{
	[LinqSyntax(
		Content = "Each MGen Generator can be used as a building block and combined using query expressions.",
		Order = 0)]
	public class LinqSyntax
	{
		[Fact]
		[LinqSyntax(
			Content =
@"F.i. the following :
```
var stringGenerator =
	from a in MGen.Int()
	from b in MGen.String()
	from c in MGen.Int()
	select a + b + c;
Console.WriteLine(stringGenerator.Generate());
```
Will output something like `28ziicuiq56`.",
			Order = 1)]
		public void SimpleCombination()
		{
			var generator =
				from a in MGen.Constant(42)
				from b in MGen.Constant("Hello")
				from c in MGen.Constant(666)
				select a + b + c;

			Assert.Equal("42Hello666", generator.Generate());
		}
		[Fact]
		[LinqSyntax(
			Content =
@"Generators are reusable building blocks. 

In the following :
```
var generator =
	from str in stringGenerator.Replace()
	from thing in MGen.One<SomeThingToGenerate>()
	select thing;
```
We reuse the 'stringGenerator' defined above and replace the default string generator with our custom one. 
All strings in the generated object will have the pattern defined by 'stringGenerator'.",
			Order = 2)]
		public void ReusingGenerators()
		{
			var generator =
				from a in MGen.Constant(42)
				from b in MGen.Constant("Hello")
				from c in MGen.Constant(666)
				select a + b + c;

			Assert.Equal("42Hello666", generator.Generate());
		}
		public class LinqSyntaxAttribute : CombiningGeneratorsAttribute
		{
			public LinqSyntaxAttribute()
			{
				Caption = "Linq Syntax.";
				CaptionOrder = 0;
			}
		}
	}
}