using System;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[Guids(
		Content = "Use `MGen.Guid()`.\n\nThere is no overload.",
		Order = 0)]
	public class GuidGeneration
	{
		[Fact]
		[Guids(
			Content = "The default generator never generates Guid.Empty.",
			Order = 2)]
		public void NeverGuidEmpty()
		{
			var generator = MGen.Guid();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
				Assert.NotEqual(Guid.Empty, val);
			}
		}

		[Fact]
		[Guids(
			Content = "Can be made to return `Guid?` using the `.Nullable()` extension.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.Guid().Nullable();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state);
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(Guid.Empty, value.Value);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		[Fact]
		[Guids(
			Content = " - `Guid` is automatically detected and generated for object properties.",
			Order = 4)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(Guid.Empty, generator.Generate(state).AProperty);
			}
		}

		[Fact]
		[Guids(
			Content = " - `Guid?` is automatically detected and generated for object properties.",
			Order = 5)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state).ANullableProperty;
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(Guid.Empty, value.Value);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		public class SomeThingToGenerate
		{
			public Guid AProperty { get; set; }
			public Guid? ANullableProperty { get; set; }
		}

		public class GuidsAttribute : ThePrimitiveGeneratorsAttribute
		{
			public GuidsAttribute()
			{
				Caption = "Guids.";
				CaptionOrder = 10;
			}
		}
	}
}