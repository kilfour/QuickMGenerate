namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Where(
		Content = "Use the `.Where(Func<T, bool>)` extension method.",
		Order = 0)]
	public class Where
	{
		[Fact]
		[Where(
			Content =
				@"Makes sure that every generated value passes the supplied predicate.",
			Order = 1)]
		public void Filters()
		{
			var generator = MGen.ChooseFromThese(1, 2, 3).Where(a => a != 1);
			for (int i = 0; i < 100; i++)
			{
				var value = generator.Generate();
				Assert.NotEqual(1, value);
			}
		}

		[Fact]
		public void WorksWithAllGenerators()
		{
			var generator = MGen.Int(1, 5).Where(a => a != 1);
			for (int i = 0; i < 100; i++)
			{
				var value = generator.Generate();
				Assert.NotEqual(1, value);
			}
		}


		public class WhereAttribute : OtherUsefullGeneratorsAttribute
		{
			public WhereAttribute()
			{
				Caption = "Filtering generated values.";
				CaptionOrder = 5;
			}
		}
	}
}