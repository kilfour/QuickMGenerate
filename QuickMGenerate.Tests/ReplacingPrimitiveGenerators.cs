using Xunit;

namespace QuickMGenerate.Tests
{
	//[ReplacingPrimitiveGenerators(Content =@"", Order = 0)]
	public class ReplacingPrimitiveGenerators
	{
		[Fact]
		public void UsesReplacement()
		{
			var generator =
				from _ in MGen.Int(42, 42).Replace()
				from result in MGen.One<SomeThingToGenerate>()
				select result;

			var value = generator.Generate();

			Assert.Equal(42, value.AnInt);
		}

		[Fact]
		public void NullableUsesReplacement()
		{
			var generator =
				from _ in MGen.Int(42, 42).Nullable().NeverReturnNull().Replace()
				from result in MGen.One<SomeThingToGenerate>()
				select result;

			var value = generator.Generate();

			Assert.Equal(42, value.ANullableProperty);
		}

		[Fact]
		public void MultipleReplacements()
		{
			var generator =
				from replace1 in MGen.Int(42, 42).Replace()
				from result1 in MGen.One<SomeThingToGenerate>()
				from replace2 in MGen.Int(666, 666).Replace()
				from result2 in MGen.One<SomeThingToGenerate>()
				select new[] { result1, result2 };

			var array = generator.Generate();

			Assert.Equal(42, array[0].AnInt);
			Assert.Equal(666, array[1].AnInt);
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
			public int? ANullableProperty { get; set; }
		}

		//public class ReplacingPrimitiveGeneratorsAttribute : GeneratingPrimitivesAttribute
		//{
		//    public ReplacingPrimitiveGeneratorsAttribute()
		//    {
		//        Caption = "Replacing Primitive Generators";
		//        CaptionOrder = 30;
		//    }
		//}
	}
}