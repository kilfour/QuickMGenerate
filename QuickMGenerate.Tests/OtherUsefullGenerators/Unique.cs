using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Unique(
		Content = "Use the `.Unique()` extension method.",
		Order = 0)]
	public class Unique
	{
		[Fact]
		[Unique(
			Content =
				@"Makes sure that every generated value is unique.",
			Order = 1)]
		public void IsUnique()
		{
			var generator = MGen.ChooseFrom(1, 2).Unique();
			var state = new State();
			var value = generator.Generate(state);
			if(value == 1)
				Assert.Equal(2, generator.Generate(state));
			else
				Assert.Equal(1, generator.Generate(state));
		}

		[Fact]
		[Unique(
			Content =
				@"When asking for more unique values than the generator can supply, an exception is thrown.",
			Order = 2)]
		public void Throws()
		{
			var generator = MGen.Constant(1).Unique();
			var state = new State();
			generator.Generate(state);
			Assert.Throws<HeyITriedFiftyTimesButCouldNotGetADifferentValue>(() => generator.Generate(state));
		}

		public class UniqueAttribute : OtherUsefullGeneratorsAttribute
		{
			public UniqueAttribute()
			{
				Caption = "Generating unique values.";
				CaptionOrder = 1;
			}
		}
	}
}