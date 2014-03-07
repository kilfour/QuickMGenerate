using Xunit;

namespace QuickMGenerate.Tests
{
	public class CustomizeTypePluggableFunctionsSpike
	{
		//private readonly IDomainGenerator domainGenerator =
		//    new DomainGenerator()
		//        .With<SomethingToGenerate>(
		//            g => g.For(
		//                e => e.MyProperty,
		//                0, val => ++val,
		//                val => string.Format("SomeString{0}", val)));

		[Fact(Skip = "WIP")]
		public void Works()
		{
			int count = 0;
			var generator =
				//from _ in MGen.Replace(MGen.Int(count++, count).AsString())
				from s in MGen.One<SomethingToGenerate>()
				select s;

			Assert.Equal("SomeString1", generator.Generate().MyProperty);
			Assert.Equal("SomeString2", generator.Generate().MyProperty);
			Assert.Equal("SomeString3", generator.Generate().MyProperty);
			Assert.Equal("SomeString4", generator.Generate().MyProperty);
			Assert.Equal("SomeString5", generator.Generate().MyProperty);
			Assert.Equal("SomeString6", generator.Generate().MyProperty);
		}

		public class SomethingToGenerate
		{
			public string MyProperty { get; set; }
		}
	}
}