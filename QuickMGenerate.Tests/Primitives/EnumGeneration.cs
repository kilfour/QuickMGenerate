using System;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[Enums(
		Content = "Use `MGen.Enum<T>()`, where T is the type of Enum you want to generate. \n\nNo overload Exists.",
		Order = 0)]
	public class EnumGeneration
	{
		[Fact]
		[Enums(
			Content = 
"The default generator just picks a random value from all enemeration values.",
			Order = 1)]
		public void DefaultGenerator()
		{
			var generator = MGen.Enum<MyEnumeration>();
			var state = new State();
			var one = false;
			var two = false;
			for (int i = 0; i < 10; i++)
			{
				var value = generator.Generate(state);
				one = one || value == MyEnumeration.MyOne;
				two = two || value == MyEnumeration.Mytwo;
			}
			Assert.True(one);
			Assert.True(two);
		}

		[Fact]
		[Enums(
			Content = " - An Enumeration is automatically detected and generated for object properties.",
			Order = 2)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var one = false;
			var two = false;
			for (int i = 0; i < 10; i++)
			{
				var value = generator.Generate(state).AnEnumeration;
				one = one || value == MyEnumeration.MyOne;
				two = two || value == MyEnumeration.Mytwo;
			}
			Assert.True(one);
			Assert.True(two);
		}

		[Fact]
		[Enums(
			Content = " - A nullable Enumeration is automatically detected and generated for object properties.",
			Order = 3)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 30; i++)
			{
				var value = generator.Generate(state).ANullableProperty;
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		[Fact]
		[Enums(
			Content = " - Passing in a non Enum type for T throws an ArgumentException.",
			Order = 3)]
		public void Throws()
		{
			Assert.Throws<ArgumentException>(() => MGen.Enum<int>().Generate());
		}

		public class SomeThingToGenerate
		{
			public MyEnumeration AnEnumeration { get; set; }
			public MyEnumeration? ANullableProperty { get; set; }
		}

		public enum MyEnumeration
		{
			MyOne,
			Mytwo
		}

		public class EnumsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public EnumsAttribute()
			{
				Caption = "Enumerations.";
				CaptionOrder = 20;
			}
		}
	}
}