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
var generator =
	from a in MGen.Int()
	from b in MGen.String()
	from c in MGen.Int()
	select a + b + c;
Console.WriteLine(generator.Generate());
```
Will output something like `28ziicuiq56`.",
			Order = 1)]
		public void FillsPrimitives()
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