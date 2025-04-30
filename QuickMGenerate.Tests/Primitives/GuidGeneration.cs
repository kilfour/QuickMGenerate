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
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate();
				Assert.NotEqual(Guid.Empty, val);
			}
		}

		[Fact]
		[Guids(
			Content = "Can be made to return `Guid?` using the `.Nullable()` combinator.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.Guid().Nullable();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 50; i++)
			{
				var value = generator.Generate();
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
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(Guid.Empty, generator.Generate().AProperty);
			}
		}

		[Fact]
		[Guids(
			Content = " - `Guid?` is automatically detected and generated for object properties.",
			Order = 5)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 50; i++)
			{
				var value = generator.Generate().ANullableProperty;
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