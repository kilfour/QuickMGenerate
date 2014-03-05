using System;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class IntGenerationTests
	{
		[Fact]
		public void Zero()
		{
			var generator = MGen.Int(0, 0);
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate());
			}
		}

		[Fact]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Int();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate());
			}
		}

		//[Fact]
		//public void UsingDomainGenerator()
		//{
		//    var generator = new DomainGenerator();
		//    100.Times(() =>
		//                {
		//                    var something = generator.One<SomethingToGenerate>();
		//                    Assert.NotEqual(0, something.PropOne);
		//                    Assert.NotEqual(0, something.PropTwo);
		//                });
		//}

		//public class SomethingToGenerate
		//{
		//    public Int32 PropOne { get; set; }
		//    public int PropTwo { get; set; }
		//}
	}
}