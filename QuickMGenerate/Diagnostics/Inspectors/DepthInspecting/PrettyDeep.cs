using QuickMGenerate;

namespace QuickMGenerate.Diagnostics.Inspectors.DepthInspecting;

public class PrettyDeep : IAmAnArtist
{
    private readonly Func<object, string> labelFunc;

    public PrettyDeep(Func<object, string> labelFunc)
    {
        this.labelFunc = labelFunc;
    }

    public Entry Sculpt(Entry entry)
    {
        var root = entry.Data;
        var lines = new List<string>();
        var stack = new Stack<(object Node, int Depth, bool IsLast, string Indent)>();
        stack.Push((root, 0, true, ""));
        while (stack.Count > 0)
        {
            var (node, depth, isLast, indent) = stack.Pop();
            var prefix = isLast ? "└── " : "├── ";
            lines.Add($"{indent}{prefix}{labelFunc(node)}");
            var props = GetPropertyInfos(node);
            string childIndent = indent + (isLast ? "    " : "│   ");
            for (int i = props.Count - 1; i >= 0; i--)
            {
                var child = props[i].GetValue(node);
                if (child != null)
                {
                    bool isLastChild = i == props.Count - 1;
                    stack.Push((child, depth + 1, isLastChild, childIndent));
                }
            }
        }
        return new Entry(entry.Tags, entry.Message, string.Join("\n", lines));
    }

    public string Sculpt(object root)
    {
        //var root = entry.Data;
        var lines = new List<string>();
        var stack = new Stack<(object Node, int Depth, bool IsLast, string Indent)>();
        stack.Push((root, 0, true, ""));
        while (stack.Count > 0)
        {
            var (node, depth, isLast, indent) = stack.Pop();
            var prefix = isLast ? "└── " : "├── ";
            lines.Add($"{indent}{prefix}{labelFunc(node)}");
            var props = GetPropertyInfos(node);
            string childIndent = indent + (isLast ? "    " : "│   ");
            for (int i = props.Count - 1; i >= 0; i--)
            {
                var child = props[i].GetValue(node);
                if (child != null)
                {
                    bool isLastChild = i == props.Count - 1;
                    stack.Push((child, depth + 1, isLastChild, childIndent));
                }
            }
        }
        return string.Join("\n", lines);
    }

    private static List<System.Reflection.PropertyInfo> GetPropertyInfos(object node)
    {
        return node.GetType()
            .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
            .Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string))
            .ToList();
    }
}
