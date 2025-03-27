namespace QuickMGenerate.Tests.Objects
{
	[ToArray(
		Content = "Use The `.ToArray()` generator extension.",
		Order = 0)]
	public class ToArray
	{
		[Fact]
		[ToArray(
			Content =
@"The `Many` generator above returns an IEnumerable.
This means it's value would be regenerated if we were to iterate over it more than once.
Use `ToArray` to *fix* the IEnumerable in place, so that it will return the same result with each iteration.
It can also be used to force evaluation in case the IEnumerable is not enumerated over because there's nothing in your select clause
referencing it. 
",
			Order = 1)]
		public void SameValues()
		{
			var values =
				(from ints in MGen.Int().Many(2).ToArray()
				 from one in MGen.Constant(ints)
				 from two in MGen.Constant(ints)
				 select new { one, two })
				.Generate();
			Assert.IsType<int[]>(values.one);
			Assert.Equal(values.one.ElementAt(0), values.two.ElementAt(0));
			Assert.Equal(values.one.ElementAt(1), values.two.ElementAt(1));
		}

		public class SomeThingToGenerate { }

		public class ToArrayAttribute : GeneratingObjectsAttribute
		{
			public ToArrayAttribute()
			{
				Caption = "ToArray.";
				CaptionOrder = 10;
			}
		}
	}
}