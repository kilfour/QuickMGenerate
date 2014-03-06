using QuickMGenerate.UnderTheHood;
using Xunit;

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
casts the generator from `Generator<State, T>` to `Generator<State, object>`. 
Usefull f.i. to generate numeric strings.",
			Order = 1)]
		public void AsString()
		{
			Assert.IsType(typeof(string), MGen.Int().AsString().Generate());
		}

		[Fact]
		[Casting(
			Content =
@" - `.AsObject()` : Simply casts the generator itself from `Generator<State, T>` to `Generator<State, object>`. Mostly used internally.",
			Order = 1)]
		public void AsObject()
		{
			Assert.IsType(typeof(Generator<State, object>), MGen.Int().AsObject());
		}

		public class CastingAttribute : OtherUsefullGeneratorsAttribute
		{
			public CastingAttribute()
			{
				Caption = "Casting Generators.";
				CaptionOrder = 1;
			}
		}
	}
}