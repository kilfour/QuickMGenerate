namespace QuickMGenerate.UnderTheHood.Diagnostics.Inspectors;

public class DepthTracker<T> : Inspector
{
    private readonly Func<T, int> _depthFunc;

    public int MaxDepth { get; private set; }

    public DepthTracker(Func<T, int> depthFunc)
    {
        _depthFunc = depthFunc;
    }

    public void Log(string[] tags, string message, object data)
    {
        if (data is T t)
            MaxDepth = Math.Max(MaxDepth, _depthFunc(t));
    }
}