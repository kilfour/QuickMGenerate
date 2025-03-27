using QuickMGenerate.Tests.Tools;

namespace QuickMGenerate.Tests.Primitives
{
	[GeneratingPrimitives(
		Content = "The MGen class has many methods which can be used to obtain a corresponding primitive.",
		Order = 0)]
	public class JustAnExample
	{
		[Fact]
		[GeneratingPrimitives(
			Content =
@"F.i. `MGen.Int()`. 

Full details below in the chapter 'The Primitive Generators'.",
			Order = 1)]
		public void ForAnInt()
		{
			Assert.NotEqual(0, MGen.Int().Generate());
		}
	}
	public class GeneratingPrimitivesAttribute : DocAttribute
	{
		public GeneratingPrimitivesAttribute()
		{
			Chapter = "Generating Primitives";
			ChapterOrder = 0;
			Caption = "Introduction";
			CaptionOrder = 0;
		}
	}
}