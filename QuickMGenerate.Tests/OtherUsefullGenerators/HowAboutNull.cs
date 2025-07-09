using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[AboutNull(
		Content = "Various extension methods allow for influencing null generation.",
		Order = 0)]
	public class HowAboutNull
	{
		[Fact]
		[AboutNull(
			Content =
@"- `.Nullable()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.  
> Used for value types.",
			Order = 1)]
		public void Nullable()
		{
			var generator = MGen.Int().Nullable();
			var seenNull = false;
			var seenValue = false;
			var tries = 0;
			while (tries++ < 1000 && !(seenNull && seenValue))
			{
				var value = generator.Generate();
				if (value is null)
					seenNull = true;
				else
					seenValue = true;
			}
			Assert.True(seenNull, "Never saw null in 1000 tries");
			Assert.True(seenValue, "Never saw non-null in 1000 tries");
		}

		[Fact]
		[AboutNull(
			Content =
@"- `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .",
			Order = 2)]
		public void NullableWithArgument()
		{
			// really don't know how to test this one
		}

		[Fact]
		[AboutNull(
			Content =
@"- `.NullableRef()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.  
> Used for reference types, including `string`.",
			Order = 3)]
		public void NullableRef()
		{
			var generator = MGen.String().NullableRef();
			var seenNull = false;
			var seenValue = false;
			var tries = 0;
			while (tries++ < 1000 && !(seenNull && seenValue))
			{
				var value = generator.Generate();
				if (value is null)
					seenNull = true;
				else
					seenValue = true;
			}
			Assert.True(seenNull, "Never saw null in 1000 tries");
			Assert.True(seenValue, "Never saw non-null in 1000 tries");
		}

		[Fact]
		[AboutNull(
			Content =
@"- `.NullableRef(int timesBeforeResultIsNullAproximation)` : overload of `NullableRef()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .",
			Order = 4)]
		public void NullableRefWithArgument()
		{
			// really don't know how to test this one
		}

		public class AboutNullAttribute : OtherUsefullGeneratorsAttribute
		{
			public AboutNullAttribute()
			{
				Caption = "How About Null(s)?";
				CaptionOrder = 11;
			}
		}
	}
}