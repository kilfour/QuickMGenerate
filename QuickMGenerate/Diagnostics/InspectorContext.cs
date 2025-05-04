namespace QuickMGenerate.Diagnostics;

public static class InspectorContext
{
    [ThreadStatic]
    public static IAmAnInspector? Current;

    public static T SetCurrent<T>(T inspector) where T : IAmAnInspector
    {
        Current = inspector;
        return inspector;
    }

    public static void Log(string[] tags, string message, object data)
    {
        Current?.Log(new(tags, message, data));
    }
}