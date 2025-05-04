using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Diagnostics;

public static partial class MGenInspecting
{
	public static Generator<T> Inspect<T>(this Generator<T> generator)
	{
		return generator.Inspect(value => (Array.Empty<string>(), "", value!));
	}

	public static Generator<T> Inspect<T>(this Generator<T> generator, string[] tags)
	{
		return generator.Inspect(value => (tags, "", value!));
	}

	public static Generator<T> Inspect<T>(this Generator<T> generator, string[] tags, string message)
	{
		return generator.Inspect(value => (tags, message, value!));
	}

	public static Generator<T> Inspect<T>(
		this Generator<T> generator,
		Func<T, (string[] tags, string message, object data)> describe)
	{
		return
			from value in generator
			from _ in Log(describe(value).tags, describe(value).message, describe(value).data)
			select value;
	}

	public static Generator<Unit> Log(string[] tags, string message, object data)
	{
		return state =>
		{
			InspectorContext.Log(tags, message, data);
			return new Result<Unit>(Unit.Instance, state);
		};
	}

}