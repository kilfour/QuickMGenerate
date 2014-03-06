using Xunit;

namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Constant(
		Content = "Use `MGen.Constant<T>(T value)`.",
		Order = 0)]
	public class Constant
	{
		[Fact]
		[Constant(
			Content = 
@"This generator is most usefull in combination with others and is used to inject constants into combined generators.",
			Order = 1)]
		public void JustReturnsValue()
		{
			Assert.Equal(42, MGen.Constant(42).Generate());
		}

		public class ConstantAttribute : OtherUsefullGeneratorsAttribute
		{
			public ConstantAttribute()
			{
				Caption = "'Generating' constants.";
				CaptionOrder = 0;
			}
		}
	}
}