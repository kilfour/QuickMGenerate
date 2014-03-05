using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, object> Apply(Action action)
		{
			action();
			return s => new Result<State, object>(null, s);
		}
	}
}