using Xunit;

namespace QuickMGenerate.Tests.Objects
{
	[SimpleObject(
		Content = "Use `MGen.One<T>()`, where T is the type of object you want to generate.",
		Order = 0)]
	public class Simple
	{
		[Fact]
		[SimpleObject(
			Content = 
@"The primitive properties of the object will be automatically filled in using the default (or replaced) generators.",
			Order = 1)]
		public void FillsPrimitives()
		{
			Assert.NotEqual(0, MGen.One<SomeThingToGenerate>().Generate().AProperty);
		}

		public class SomeThingToGenerate
		{
			public int AProperty { get; set; }
		}

		public class SimpleObjectAttribute : GeneratingObjectsAttribute
		{
			public SimpleObjectAttribute()
			{
				Caption = "A simple object.";
				CaptionOrder = 0;
			}
		}
	}
}