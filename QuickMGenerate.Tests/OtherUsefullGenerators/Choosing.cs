using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Choosing(
		Content = "Use `MGen.ChooseFrom<T>(params T[] values)`.",
		Order = 0)]
	public class Choosing
	{
		[Fact]
		[Choosing(
			Content =
@"Picks a random value from a list of options.

F.i. `MGen.ChooseFrom(1, 2)` will return either 1 or 2.",
			Order = 1)]
		public void JustReturnsValue()
		{
			var generator = MGen.ChooseFrom(1, 2);
			var state = new State();
			var one = false;
			var two = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state);
				one = one || value == 1;
				two = two || value == 2;
			}
			Assert.True(one);
			Assert.True(two);
		}

		public class ChoosingAttribute : OtherUsefullGeneratorsAttribute
		{
			public ChoosingAttribute()
			{
				Caption = "Picking an element out of a range.";
				CaptionOrder = 1;
			}
		}
	}
}