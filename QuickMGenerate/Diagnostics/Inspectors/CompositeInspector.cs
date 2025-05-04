namespace QuickMGenerate.Diagnostics.Inspectors;

public class CompositeInspector : IAmAnInspector
{
    private readonly List<IAmAnInspector> _inspectors = new();

    public CompositeInspector(params IAmAnInspector[] inspectors)
    {
        _inspectors.AddRange(inspectors);
    }

    public void Add(IAmAnInspector inspector) => _inspectors.Add(inspector);

    public void Log(Entry entry)
    {
        foreach (var inspector in _inspectors)
            inspector.Log(entry);
    }
}