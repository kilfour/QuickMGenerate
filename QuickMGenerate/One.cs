using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T> One<T>()
		{
			return s => new Result<State, T>(Activator.CreateInstance<T>(), s);
		}
	}
}