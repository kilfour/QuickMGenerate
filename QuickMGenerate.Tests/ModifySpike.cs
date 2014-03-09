using Xunit;

namespace QuickMGenerate.Tests
{
	public class ModifySpike
	{
		[Fact]
		public void FirstShot()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			for (int i = 0; i < 10; i++)
			{
				var result = generator.Modify(new SomeThingToGenerate { AnInt = 42 }).Generate();
				Assert.NotEqual(42, result.AnInt);
				Assert.True(result.ABool);
			}
		}

		[Fact]
		public void Ints()
		{
			var generator = MGen.Int();
			for (int i = 0; i < 10; i++)
			{
				var result = generator.ModifyPrimitive(42).Generate();
				Assert.NotEqual(42, result);
			}
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
			public bool ABool { get; set; }
		}
	}
}