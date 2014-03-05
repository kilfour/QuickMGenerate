﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class Spike
	{
		[Fact]
		public void FirstShot()
		{
			var x =
				from a in MGen.Int()
				from b in MGen.String()
				from c in MGen.Int()
				select a + b + c;

			Console.WriteLine(x.Generate());

			var childGenerator =
				from i in MGen.Int()
				select new SomeChild { AnInt = i };

			var state = new State();

			Console.WriteLine(childGenerator.Generate(state).AnInt);
			Console.WriteLine(childGenerator.Generate(state).AnInt);
			Console.WriteLine(childGenerator.Generate(state).AnInt);

			var generator =
				from i in MGen.Int()
				from str in MGen.String()
				from children in childGenerator.Many(5)
				from stg in MGen.One<SomeThingToGenerate>()
				from _ in MGen.Do(stg, s3 => s3.AString = str + i)
				from __ in MGen.Do(stg, s3 => children.Select(c => { s3.Children.Add(c); return 0; }).ToArray())
				select stg;

			var result = generator.Generate(state);
			Console.WriteLine(result.AString);
			foreach (var someChild in result.Children)
			{
				Console.WriteLine(someChild.AnInt);
			}

		}

		public class SomeThingToGenerate
		{
			public string AString { get; set; }
			public List<SomeChild> Children { get; set; }

			public SomeThingToGenerate()
			{
				Children = new List<SomeChild>();
			}
		}

		public class SomeChild
		{
			public int AnInt { get; set; }
		}
	}
}