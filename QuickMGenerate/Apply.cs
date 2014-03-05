using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, Unit> Apply(Action action)
		{
			action();
			return s => new Result<State, Unit>(Unit.Instance, s);
		}
	}
}