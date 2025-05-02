namespace QuickMGenerate.UnderTheHood.Diagnostics.Inspectors;

public class NullabilityInspector<T> : Inspector
{
    public bool SawNull { get; private set; }
    public bool SawNonNull { get; private set; }

    public void Log(string[] tags, string message, object data)
    {
        if (data is null) SawNull = true;
        else SawNonNull = true;
    }
}