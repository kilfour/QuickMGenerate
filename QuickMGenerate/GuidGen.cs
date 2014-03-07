using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, Guid> Guid()
		{
			return s => new Result<State, Guid>(System.Guid.NewGuid(), s);
		}
	}
}