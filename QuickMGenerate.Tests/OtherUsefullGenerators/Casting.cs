using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Casting(
		Content = "Various extension methods allow for casting the generated value.",
		Order = 0)]
	public class Casting
	{
		[Fact]
		[Casting(
			Content =
@" - `.AsString()` : Invokes `.ToString()` on the generated value and 
casts the generator from `Generator<T>` to `Generator<string>`. 
Usefull f.i. to generate numeric strings.",
			Order = 1)]
		public void AsString()
		{
			Assert.IsType<string>(MGen.Int().AsString().Generate());
		}

		[Fact]
		[Casting(
			Content =
@" - `.AsObject()` : Simply casts the generator itself from `Generator<T>` to `Generator<object>`. Mostly used internally.",
			Order = 1)]
		public void AsObject()
		{
			Assert.IsType<Generator<object>>(MGen.Int().AsObject());
		}

		[Fact]
		[Casting(
			Content =
@" - `.Nullable()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.",
			Order = 1)]
		public void Nullable()
		{
			var generator = MGen.Int().Nullable();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 30; i++)
			{
				var value = generator.Generate();
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
		[Casting(
			Content =
@" - `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .",
			Order = 1)]
		public void NullableWithArgument()
		{
			// really don't know how to test this one
		}

		public class CastingAttribute : OtherUsefullGeneratorsAttribute
		{
			public CastingAttribute()
			{
				Caption = "Casting Generators.";
				CaptionOrder = 10;
			}
		}
	}
}