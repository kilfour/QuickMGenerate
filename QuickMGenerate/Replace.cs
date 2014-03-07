using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, Unit> Replace<T>(this Generator<State, T> generator)
			where T : struct
		{
			return s =>
			{
				s.PrimitiveGenerators[typeof(T)] = generator.AsObject();
				s.PrimitiveGenerators[typeof(T?)] = generator.Nullable().AsObject();
				return new Result<State, Unit>(Unit.Instance, s);
			};
		}
	}
}