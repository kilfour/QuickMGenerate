using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace QuickMGenerate.Tests.Objects
{
	[ManyObjects(
		Content = "Use The `.Many(int number)` generator extension.",
		Order = 0)]
	public class ManyObjects
	{
		[Fact]
		[ManyObjects(
			Content = 
@"The generator will generate an IEnumerable<T> of `int number` elements where T is the result type of the extended generator.",
			Order = 1)]
		public void CorrectAmountOfElements()
		{
			var values = MGen.One<SomeThingToGenerate>().Many(2).Generate();
			Assert.Equal(2, values.Count());
			Assert.IsAssignableFrom(typeof(IEnumerable<SomeThingToGenerate>), values);
		}

	    [Fact]
	    [ManyObjects(
	        Content =
@"An overload exists (`.Many(int min, int max`) where the number of elements is in between the specified arguments.",
	        Order = 1)]
	    public void CorrectAmountOfElementsMinMax()
	    {
	        var values = MGen.One<SomeThingToGenerate>().Many(2, 2).Generate();
	        Assert.Equal(2, values.Count());
	        Assert.IsAssignableFrom(typeof(IEnumerable<SomeThingToGenerate>), values);
	    }

        public class SomeThingToGenerate { }

		public class ManyObjectsAttribute : GeneratingObjectsAttribute
		{
			public ManyObjectsAttribute()
			{
				Caption = "Many objects.";
				CaptionOrder = 10;
			}
		}
	}
}