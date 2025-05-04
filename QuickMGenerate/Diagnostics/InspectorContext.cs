namespace QuickMGenerate.Diagnostics;

public static class InspectorContext
{
    [ThreadStatic]
    public static Inspector? Current;

    public static void Log(string[] tags, string message, object data)
    {
        Current?.Log(tags, message, data);
    }
}