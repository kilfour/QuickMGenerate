using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<Guid> Guid()
		{
			return s => new Result<Guid>(System.Guid.NewGuid(), s);
		}
	}
}