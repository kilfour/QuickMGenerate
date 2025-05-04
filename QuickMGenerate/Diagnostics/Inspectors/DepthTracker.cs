using QuickMGenerate;
using QuickMGenerate.Diagnostics.Inspectors.Calipers;

namespace QuickMGenerate.Diagnostics.Inspectors;
public record DepthEntry(string Path, string Type, int Depth);
internal static class Depth
{
    internal static IEnumerable<DepthEntry> Inspect(object root)
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

    public static string ToPrettyStringWithPipes(
        object root,
        Func<object, string> labelFunc)
    {
        var lines = new List<string>();
        var stack = new Stack<(object Node, int Depth, bool IsLast, string Indent)>();
        stack.Push((root, 0, true, ""));

        while (stack.Count > 0)
        {
            var (node, depth, isLast, indent) = stack.Pop();
            var prefix = isLast ? "└── " : "├── ";
            lines.Add($"{indent}{prefix}{labelFunc(node)}");


            var props = node.GetType()
                .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string))
                .ToList();
            string childIndent = indent + (isLast ? "    " : "│   ");
            // foreach (var prop in props)
            // {
            //     var value = prop.GetValue(node);
            //     if (value != null)
            //     {
            //         queue.Enqueue((value, $"{path}.{prop.Name}", depth + 1));
            //     }
            // }


            for (int i = props.Count - 1; i >= 0; i--)
            {
                bool isLastChild = i == props.Count - 1;
                stack.Push((props[i]!, depth + 1, isLastChild, childIndent));
            }
        }

        return string.Join("\n", lines);
    }
}

public class DepthTracker : IAmAnInspector
{
    public List<List<DepthEntry>> Depths { get; private set; } = [];
    public void Log(Entry entry)
    {
        Depths.Add([.. Depth.Inspect(entry.Data)]);
    }
}

public class DepthSculpter : IReshaper
{
    public Entry Sculpt(Entry entry)
    {
        (string[] tags, string message, object data) = entry;
        return new(
            tags.Append("Depth").ToArray(),
            message,
            Depth.Inspect(data).ToArray());
    }
}