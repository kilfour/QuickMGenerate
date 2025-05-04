namespace QuickMGenerate.Diagnostics.Inspectors;

public class DistinctValueInspector<T> : Inspector
{
    public readonly HashSet<T> Seen = new();

    public void Log(string[] tags, string message, object data)
    {
        if (data is T t)
            Seen.Add(t);
    }
}