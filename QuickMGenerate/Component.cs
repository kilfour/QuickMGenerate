using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, Unit> Component<T>(this Generator<State, T> generator)
		{
			return s =>
			{
				s.Components[typeof(T)] = generator.AsObject();
				return new Result<State, Unit>(Unit.Instance, s);
			};
		}
	}
}