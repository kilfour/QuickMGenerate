using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, Unit> Replace<T>(Generator<State, T> generator)
		{
			return s =>
			       	{
			       		s.PrimitiveGenerators[typeof (T)] = generator.AsObject();
			       		return new Result<State, Unit>(Unit.Instance, s);
			       	};
		}
	}
}