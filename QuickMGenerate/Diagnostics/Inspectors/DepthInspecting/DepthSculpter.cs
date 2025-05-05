using QuickMGenerate;

namespace QuickMGenerate.Diagnostics.Inspectors.DepthInspecting;

public class DepthSculpter : IAmAnArtist
{
    public record DepthEntry(string Path, string Type, int Depth);

    public Entry Sculpt(Entry entry)
    {
        (string[] tags, string message, object data) = entry;
        return new(
            tags.Append("Depth").ToArray(),
            message,
            InspectDepth(data).ToArray());
    }

    private static IEnumerable<DepthEntry> InspectDepth(object root)
    {
        var queue = new Queue<(object Node, string Path, int Depth)>();
        queue.Enqueue((root, "Root", 1));

        while (queue.Count > 0)
        {
            var (node, path, depth) = queue.Dequeue();
            yield return new(path, node.GetType().Name, depth);

            var props = node.GetType()
                .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string));

            foreach (var prop in props)
            {
                var value = prop.GetValue(node);
                if (value != null)
                {
                    queue.Enqueue((value, $"{path}.{prop.Name}", depth + 1));
                }
            }
        }
    }
}