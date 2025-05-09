using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate;

public static partial class MGenInspecting
{
	public static Generator<T> Inspect<T>(this Generator<T> generator)
	{
		return
			from value in generator
			from _ in Log(value)
			select value;
	}

	private static Generator<Unit> Log(object data)
	{
		return state =>
		{
			InspectContext.Current?.Invoke([data]);
			return new Result<Unit>(Unit.Instance, state);
		};
	}
}

public static class InspectContext
{
	[ThreadStatic]
	public static Action<object[]>? Current;
}