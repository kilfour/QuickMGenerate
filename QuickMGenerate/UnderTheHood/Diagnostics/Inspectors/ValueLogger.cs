namespace QuickMGenerate.UnderTheHood.Diagnostics.Inspectors;

public class ValueLogger<T> : Inspector
{
    public readonly List<T> Values = new();
    private readonly int _max;

    public ValueLogger(int max = 100)
    {
        _max = max;
    }

    public void Log(string[] tags, string message, object data)
    {
        if (data is T t && Values.Count < _max)
            Values.Add(t);
    }
}