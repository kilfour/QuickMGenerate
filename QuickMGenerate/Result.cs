namespace QuickMGenerate
{
	public class Result<TState, TValue>
	{
		public readonly TValue Value;
		public readonly TState State;
		public Result(TValue value, TState state) { Value = value; State = state; }
	}
}