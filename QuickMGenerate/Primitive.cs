using System;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<object> Primitive(Type type)
		{
			return s => s.PrimitiveGenerators[type](s);
		}
	}
}