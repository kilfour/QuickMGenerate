using QuickMGenerate.UnderTheHood;
using QuickPulse.Diagnostics;

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
			PulseContext.Log(data);
			return new Result<Unit>(Unit.Instance, state);
		};
	}
}