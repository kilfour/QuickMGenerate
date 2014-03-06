using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class StringBuilderSpike
	{
		[Fact]
		public void FirstShot()
		{
			var stringGenerator =
				MGen.Int(1, 1)
					.AsString()
					.Append("test")
					.Append(MGen.Int(42, 42).AsString());
			var result = stringGenerator.Generate();
			Assert.Equal("1test42", result);
		}
	}
}