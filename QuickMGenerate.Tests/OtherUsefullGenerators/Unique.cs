﻿using QuickMGenerate.UnderTheHood;
using Xunit;

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
			var generator = MGen.ChooseFrom(1, 2).Unique("TheKey");
			for (int i = 0; i < 100; i++)
			{
				var state = new State();
				var value = generator.Generate(state);
				if (value == 1)
					Assert.Equal(2, generator.Generate(state));
				else
					Assert.Equal(1, generator.Generate(state));
			}
		}

		[Fact]
		[Unique(
			Content =
				@"When asking for more unique values than the generator can supply, an exception is thrown.",
			Order = 2)]
		public void Throws()
		{
			var generator = MGen.Constant(1).Unique("TheKey");
			var state = new State();
			generator.Generate(state);
			Assert.Throws<HeyITriedFiftyTimesButCouldNotGetADifferentValue>(() => generator.Generate(state));
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
				var state = new State();
				var generator =
					from one in MGen.ChooseFrom(1, 2).Unique(1)
					from two in MGen.ChooseFrom(1, 2).Unique(2)
					select new[] {one, two};
				
				var valueOne = generator.Generate(state);
				var valueTwo = generator.Generate(state);
				if (valueOne[0] == 1)
					Assert.Equal(2, valueTwo[0]);
				else
					Assert.Equal(1, valueTwo[0]);
				if (valueOne[1] == 1)
					Assert.Equal(2, valueTwo[1]);
				else
					Assert.Equal(1, valueTwo[1]);	
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
				var state = new State();
				var generator =
					from one in MGen.ChooseFrom(1, 2).Unique(1)
					from two in MGen.ChooseFrom(1, 2).Unique(1)
					select new[] {one, two};

				var valueOne = generator.Generate(state);
				if (valueOne[0] == 1)
					Assert.Equal(2, valueOne[1]);
				else
					Assert.Equal(1, valueOne[1]);
			}
		}

		public class UniqueAttribute : OtherUsefullGeneratorsAttribute
		{
			public UniqueAttribute()
			{
				Caption = "Generating unique values.";
				CaptionOrder = 2;
			}
		}
	}
}