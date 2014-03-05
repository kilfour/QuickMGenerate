using System.Reflection;

namespace QuickMGenerate.UnderTheHood
{
	public static class MyBinding
	{
		public const BindingFlags Flags =
			BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
	}
}