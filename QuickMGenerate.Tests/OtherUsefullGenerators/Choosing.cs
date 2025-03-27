namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Choosing(
		Content = "Use `MGen.ChooseFrom<T>(IEnumerable<T> values)`.",
		Order = 0)]
	public class Choosing
	{
		[Fact]
		[Choosing(
			Content =
@"Picks a random value from a list of options.

F.i. `MGen.ChooseFrom(new []{ 1, 2 })` will return either 1 or 2.",
			Order = 1)]
		public void Enumerable()
		{
			var generator = MGen.ChooseFrom(new[] { 1, 2 });
			var one = false;
			var two = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate();
				one = one || value == 1;
				two = two || value == 2;
			}
			Assert.True(one);
			Assert.True(two);
		}

		[Fact]
		[Choosing(
			Content =
@"A helper method exists for ease of use when you want to pass in constant values as in the example above. 

I.e. : `MGen.ChooseFromThese(1, 2)`",
			Order = 2)]
		public void Params()
		{
			var generator = MGen.ChooseFromThese(1, 2);
			var one = false;
			var two = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate();
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
				CaptionOrder = 2;
			}
		}
	}
}