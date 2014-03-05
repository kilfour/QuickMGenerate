namespace QuickMGenerate.UnderTheHood
{
	public delegate IResult<TState, TValue> Generator<TState, out TValue>(TState input);
}
