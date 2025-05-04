namespace QuickMGenerate.Tests.OnScope;

[GeneralInfo(
	Content =
@"Linq chains especially in query syntax can be confusing when it comes to scope.
But once you understand the basic rule, everything will quickly seem obvious.
For generating trivial and even less than trivial examples, you can likely ignore this chapter completely.
However for complex stuff where generators get reused, sometimes in implicit ways, 
it is good to know what *exactly* is going on.
",
	Order = 0)]
public class GeneralInfoTests
{
	[GeneralInfo(
	Content =
@"**Note:** This section is still being worked on, more information will follow.",
	Order = 0)]
	[Fact]
	public void UsageExample()
	{
		var generator =
			from val in MGen.Int()
			from v1 in MGen.Constant(val)
			from v2 in MGen.Constant(val)
			select (v1, v2);

	}

	public class GeneralInfoAttribute : OnScopeAttribute
	{
		public GeneralInfoAttribute()
		{
			Caption = "Overview";
			CaptionOrder = 0;
		}
	}
}