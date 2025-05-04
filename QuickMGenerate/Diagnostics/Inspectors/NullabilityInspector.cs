namespace QuickMGenerate.Diagnostics.Inspectors;

public class NullabilityInspector<T> : IAmAnInspector
{
    public bool SawNull { get; private set; }
    public bool SawNonNull { get; private set; }

    public void Log(Entry entry)
    {
        if (entry.Data is null) SawNull = true;
        else SawNonNull = true;
    }
}