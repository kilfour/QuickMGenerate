using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class NumericStringSpike
	{
		[Fact]
		public void FirstShot()
		{
			var state = new State();
			for (int i = 0; i < 100; i++)
			{
				var numberAsString = MGen.Int().AsString().Generate(state);
				Assert.IsType(typeof(string), numberAsString);
				var number = int.Parse(numberAsString);
				Assert.InRange(number, 1, 99);	
			}
		}
	}
}