using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		// public static Generator<TBase> Tree<TBase>(int maxDepth)
		// {
		// 	if (maxDepth < 1)
		// 		throw new ArgumentException($"Invalid argument : maxDepth ({maxDepth}) < 1");
		// 	return s =>
		// 	{
		// 		s.Components.Add(typeof(TBase));
		// 		s.RecursionRules[typeof(TBase)] = (maxDepth, null);
		// 		// if (maxDepth == 1)
		// 		// 	return new Result<TBase>(One<TEnd>()(s).Value, s);
		// 		return One<TBase>()(s);
		// 	};
		// }

		// public static Generator<TBase> Tree<TBase>(int maxDepth, params Type[] derivedTypes) where TEnd : TBase
		// {
		// 	if (maxDepth < 1)
		// 		throw new ArgumentException($"Invalid argument : maxDepth ({maxDepth}) < 1");
		// 	return s =>
		// 	{
		// 		s.Components.Add(typeof(TBase));
		// 		s.InheritanceInfo[typeof(TBase)] = derivedTypes.ToList();
		// 		s.RecursionRules[typeof(TBase)] = (maxDepth, typeof(TEnd));
		// 		// if (maxDepth == 1)
		// 		// 	return new Result<TBase>(One<TEnd>()(s).Value, s);
		// 		return One<TBase>()(s);
		// 	};
		// }

		public static Generator<TBase> Tree<TBase, TEnd>(int maxDepth, params Type[] derivedTypes) where TEnd : TBase
		{
			if (maxDepth < 1)
				throw new ArgumentException($"Invalid argument : maxDepth ({maxDepth}) < 1");
			return s =>
			{
				s.Components.Add(typeof(TBase));
				s.InheritanceInfo[typeof(TBase)] = derivedTypes.ToList();
				s.RecursionRules[typeof(TBase)] = (maxDepth, typeof(TEnd));
				if (maxDepth == 1)
					return new Result<TBase>(One<TEnd>()(s).Value, s);
				return One<TBase>()(s);
			};
		}

	}
}