using QuickPulse;

namespace QuickMGenerate.Tests._Tools;

public class DistinctValueInspector<T> : IArtery
{
    public readonly HashSet<T> Seen = new();

    public bool HasSeen(T value)
    {
        return Seen.Contains(value);
    }

    public bool AllTheseHaveBeenSeen(T[] values)
    {
        return values.All(a => Seen.Contains(a));
    }

    public bool HasValueThatSatisfies(Func<T, bool> predicate)
    {
        return Seen.Any(predicate);
    }

    public bool SeenSatisfyEach(Func<T, bool>[] predicates)
    {
        return predicates.All(predicate => Seen.Any(value => predicate(value)));
    }

    public void Flow(params object[] data)
    {
        foreach (var item in data)
        {
            if (item is T || item == null)
                Seen.Add((T?)item!);
        }
    }
}
