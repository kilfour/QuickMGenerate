using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{

		public static Generator<TBase> Tree<TBase, TEnd>(int maxDepth, params Type[] derivedTypes) where TEnd : TBase
		{
			if (maxDepth < 1)
				throw new ArgumentException($"Invalid argument : maxDepth ({maxDepth}) < 1");
			return s =>
			{
				s.DepthConstraints[typeof(TBase)] = new(maxDepth, maxDepth);
				s.InheritanceInfo[typeof(TBase)] = derivedTypes.ToList();
				s.RecursionRules[typeof(TBase)] = (maxDepth, typeof(TEnd));
				return One<TBase>()(s);
			};
		}

	}
}