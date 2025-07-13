namespace QuickMGenerate.Tests._SandBox;

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
	public void Ignoring()
	{
		var generator =
			from _ in MGen.For<SomeThingToGenerate>().Ignore(e => e.AnInt)
			from one in MGen.One<SomeThingToGenerate>()
			select one;
		for (int i = 0; i < 10; i++)
		{
			var result = generator.Modify(new SomeThingToGenerate { AnInt = 42 }).Generate();
			Assert.Equal(42, result.AnInt);
		}
	}

	public class SomeThingToGenerate
	{
		public int AnInt { get; set; }
		public bool ABool { get; set; }
	}
}