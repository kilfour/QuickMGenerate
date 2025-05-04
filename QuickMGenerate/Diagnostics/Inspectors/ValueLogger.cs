namespace QuickMGenerate.Diagnostics.Inspectors;

public class ValueLogger<T> : IAmAnInspector
{
    public readonly List<T> Values = new();
    private readonly int _max;

    public ValueLogger(int max = 100)
    {
        _max = max;
    }
    public void Log(Entry entry)
    {
        if (entry.Data is T t && Values.Count < _max)
            Values.Add(t);
    }
}

public class ValueCapture<T>
{
    public readonly List<T> Values = new();
    private readonly int _max;

    public ValueCapture(int max = 100)
    {
        _max = max;
    }

    public void Accept(object data)
    {
        if (data is T t && Values.Count < _max)
            Values.Add(t);
    }
}

