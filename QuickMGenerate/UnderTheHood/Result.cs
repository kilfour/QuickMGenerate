namespace QuickMGenerate.UnderTheHood
{
	public interface IResult<TState, out TValue>
	{
		TValue Value { get; }
	}

	public class Result<TState, TValue> : IResult<TState, TValue>
	{
		public TValue Value { get; private set; }
		public readonly TState State;
		public Result(TValue value, TState state) { Value = value; State = state; }
	}
}