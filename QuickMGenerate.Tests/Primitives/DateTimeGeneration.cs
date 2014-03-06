﻿using System;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.Tests.Primitives
{
	[DateTimes(
		Content = "Use `MGen.DateTime()`.",
		Order = 0)]
	public class DateTimeGeneration
	{
		[Fact]
		[DateTimes(
			Content = "The overload `MGen.DateTimes(DateTime min, DateTime max)` generates a DateTime higher or equal than min and lower than max.",
			Order = 1)]
		public void Zero()
		{
			var generator = MGen.DateTime(new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.True(generator.Generate(state) >= new DateTime(2000, 1, 1));
				Assert.True(generator.Generate(state) < new DateTime(2000, 1, 5));
			}
		}

		[Fact]
		[DateTimes(
			Content = "The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).",
			Order = 2)]
		public void DefaultGeneratorNeverGeneratesZero()
		{
			var generator = MGen.DateTime();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				var val = generator.Generate(state);
				Assert.True(val > new DateTime(1970, 1, 1));
				Assert.True(val < new DateTime(2020, 12, 31));
			}
		}

		[Fact]
		[DateTimes(
			Content = "Can be made to return `DateTime?` using the `.Nullable()` extension.",
			Order = 3)]
		public void Nullable()
		{
			var generator = MGen.DateTime().Nullable();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state);
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(new DateTime(), value.Value);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		[Fact]
		[DateTimes(
			Content = " - `DateTime` is automatically detected and generated for object properties.",
			Order = 4)]
		public void Property()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			for (int i = 0; i < 10; i++)
			{
				Assert.NotEqual(new DateTime(), generator.Generate(state).AProperty);
			}
		}

		[Fact]
		[DateTimes(
			Content = " - `DateTime?` is automatically detected and generated for object properties.",
			Order = 5)]
		public void NullableProperty()
		{
			var generator = MGen.One<SomeThingToGenerate>();
			var state = new State();
			var isSomeTimesNull = false;
			var isSomeTimesNotNull = false;
			for (int i = 0; i < 20; i++)
			{
				var value = generator.Generate(state).ANullableProperty;
				if (value.HasValue)
				{
					isSomeTimesNotNull = true;
					Assert.NotEqual(new DateTime(), value.Value);
				}
				else
					isSomeTimesNull = true;
			}
			Assert.True(isSomeTimesNull);
			Assert.True(isSomeTimesNotNull);
		}

		public class SomeThingToGenerate
		{
			public DateTime AProperty { get; set; }
			public DateTime? ANullableProperty { get; set; }
		}

		public class DateTimesAttribute : GeneratingPrimitivesAttribute
		{
			public DateTimesAttribute()
			{
				Caption = "DateTimes";
				CaptionOrder = 6;
			}
		}
	}
}