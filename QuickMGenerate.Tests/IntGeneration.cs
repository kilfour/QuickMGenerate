﻿using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class IntGeneration
	{
		[Fact]
		public void Zero()
		{
			var generator = MGen.Int(0, 0);
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.Equal(0, generator.Generate(state));
			}
		}

		[Fact]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.Int();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state));
			}
		}

		[Fact]
		public void Nullable()
		{
			var generator = MGen.Int().Nullable();
			var state = new State();
			var isSomeTimesNull = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state);
				if (value.HasValue)
					Assert.NotEqual(0, value);
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
		}

		[Fact]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(0, generator.Generate(state).AnInt);
			}
		}

		[Fact]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			for (int i = 0; i < 10; i++)
			{
				var value = generator.Generate(state).ANullableInt;
				if (value.HasValue)
					Assert.NotEqual(0, value);
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
		}

		public class SomeThingToGenerate
		{
			public int AnInt { get; set; }
			public int? ANullableInt { get; set; }
		}
	}
}