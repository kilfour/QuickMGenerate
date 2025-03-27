namespace QuickMGenerate.Tests.OtherUsefullGenerators
{
	[Unique(
		Content = "Use the `.Unique(object key)` extension method.",
		Order = 0)]
	public class Unique
	{
		[Fact]
		[Unique(
			Content =
				@"Makes sure that every generated value is unique.",
			Order = 1)]
		public void IsUnique()
		{
			var generator = MGen.ChooseFromThese(1, 2).Unique("TheKey").Many(2);
			for (int i = 0; i < 100; i++)
			{
				var value = generator.Generate().ToArray();
				Assert.Equal(value[0] == 1 ? 2 : 1, value[1]);
			}
		}

		[Fact]
		[Unique(
			Content =
				@"When asking for more unique values than the generator can supply, an exception is thrown.",
			Order = 2)]
		public void Throws()
		{
			var generator = MGen.Constant(1).Unique("TheKey").Many(2);
			Assert.Throws<HeyITriedFiftyTimesButCouldNotGetADifferentValue>(() => generator.Generate().ToArray());
		}

		[Fact]
		[Unique(
			Content =
@"Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.",
			Order = 3)]
		public void Multiple()
		{
			for (int i = 0; i < 100; i++)
			{
				var generator =
					from one in MGen.ChooseFromThese(1, 2).Unique(1)
					from two in MGen.ChooseFromThese(1, 2).Unique(2)
					select new[] { one, two };
				var value = generator.Many(2).Generate().ToArray();
				var valueOne = value[0];
				var valueTwo = value[1];
				Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueTwo[0]);
				Assert.Equal(valueOne[1] == 1 ? 2 : 1, valueTwo[1]);
			}

		}

		[Fact]
		[Unique(
			Content =
@"When using the same key for multiple unique generators all values across these generators are unique.",
			Order = 4)]
		public void MultipleSameKey()
		{
			for (int i = 0; i < 100; i++)
			{
				var generator =
					from one in MGen.ChooseFromThese(1, 2).Unique(1)
					from two in MGen.ChooseFromThese(1, 2).Unique(1)
					select new[] { one, two };

				var valueOne = generator.Generate();
				Assert.Equal(valueOne[0] == 1 ? 2 : 1, valueOne[1]);
			}
		}

		public class UniqueAttribute : OtherUsefullGeneratorsAttribute
		{
			public UniqueAttribute()
			{
				Caption = "Generating unique values.";
				CaptionOrder = 4;
			}
		}
	}
}