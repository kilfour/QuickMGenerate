using Xunit;

namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[NeverReturnNull(
		Content = "Use the `.NeverReturnNull()` extension method.`.",
		Order = 0)]
	public class NeverReturnNull
	{
		[Fact]
		[NeverReturnNull(
			Content = 
@"Only available on generators that provide `Nullable<T>` values, this one makes sure that, you guessed it, the nullable generator never returns null.",
			Order = 1)]
		public void NeverNull()
		{
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(42, MGen.Constant(42).Nullable().NeverReturnNull().Generate());	
			}
		}

		public class NeverReturnNullAttribute : OtherUsefullGeneratorsAttribute
		{
			public NeverReturnNullAttribute()
			{
				Caption = "'Never return null.";
				CaptionOrder = 20;
			}
		}
	}
}