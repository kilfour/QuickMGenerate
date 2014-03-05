namespace QuickMGenerate.UnderTheHood
{
	public delegate Result<TState, TValue> Generator<TState, TValue>(TState input);
}
