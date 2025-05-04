namespace QuickMGenerate.Diagnostics.Inspectors;

public class DistinctValueInspector<T> : IAmAnInspector
{
    public readonly HashSet<T> Seen = new();

    public void Log(Entry entry)
    {
        if (entry.Data is T t)
            Seen.Add(t);
    }
}