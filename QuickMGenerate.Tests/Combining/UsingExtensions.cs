using System.Linq;
using QuickMGenerate.Tests.Objects;
using Xunit;

namespace QuickMGenerate.Tests.Combining
{
	[UsingExtensions(
		Content = "When applying the various extension methods onto a generator, they get *combined* into a new generator.",
		Order = 0)]
	public class UsingExtensions
	{
		[Fact]
		[UsingExtensions(
			Content =
@"Jumping slightly ahead of ourselves as below example will use methods that are explained more thoroughly further below.

The old quickgenerate had a *PickOne()* method, which randomly picked an element from an IEnumerable.

This has now been replaced with `MGen.ChooseFrom()` and `MGen.ChooseFromThese()` (see Chapter 'Other Usefull Generators').

QuickGenerate also had a *PickMany(int number)* method which picked *number* amount of elements from an IEnumerable 
and also made sure that it picked different elements.

The PickMany method is now obsolete as the same thing can be achieved through generator composition.

E.g. :
```
MGen.ChooseFrom(someValues).Unique(""key"").Many(2)
```

In the same vein, I was able to leave a lot of code out, and at the same time, probably providing more features.
",
			Order = 1)]
		public void SimpleCombination()
		{
			var generator =
				from a in MGen.ChooseFromThese(1, 2).Unique("key").Many(2)
				select a;
			for (int i = 0; i < 10; i++)
			{
				var values = generator.Generate().ToArray();
				Assert.Equal(values[0] == 1 ? 2 : 1, values[1]);	
			}
		}

		public class UsingExtensionsAttribute : CombiningGeneratorsAttribute
		{
			public UsingExtensionsAttribute()
			{
				Caption = "Using Extensions.";
				CaptionOrder = 1;
			}
		}
	}
}