namespace QuickMGenerate.Diagnostics.Inspectors.Calipers;

// Core structural walker
public static class TreeWalker
{
    public static IEnumerable<TResult> Walk<T, TResult>(
        T? root,
        Func<T, IEnumerable<T?>> getChildren,
        Func<T, int, TResult> mapFunc,
        int startDepth = 1)
    {
        if (root == null)
            yield break;

        var stack = new Stack<(T Node, int Depth)>();
        stack.Push((root, startDepth));

        while (stack.Count > 0)
        {
            var (node, depth) = stack.Pop();
            yield return mapFunc(node, depth);

            var children = getChildren(node).Where(c => c != null).Reverse();
            foreach (var child in children)
                stack.Push((child!, depth + 1));
        }
    }
}

// Consumer #1: DepthsFunc (for leaf depth assertions)
public static class TreeUtils
{
    public static Func<T, IEnumerable<int>> DepthsFunc<T>(
        Func<T, IEnumerable<T?>> getChildren) =>
        root => TreeWalker
            .Walk(root, getChildren, (node, depth) => (Node: node, Depth: depth))
            .Where(t => !getChildren(t.Node).Any()) // leaf node
            .Select(t => t.Depth);
}

// Consumer #2: Visualizer
public static class TreeVisualizer
{
    public static string ToPrettyString<T>(
        T root,
        Func<T, IEnumerable<T?>> getChildren,
        Func<T, int, string> formatNode)
    {
        var lines = TreeWalker
            .Walk(root, getChildren, formatNode)
            .ToList();

        return string.Join("\n", lines);
    }

    public static string ToPrettyStringWithPipes<T>(
        T root,
        Func<T, IEnumerable<T?>> getChildren,
        Func<T, string> labelFunc)
    {
        var lines = new List<string>();
        var stack = new Stack<(T Node, int Depth, bool IsLast, string Indent)>();
        stack.Push((root, 0, true, ""));

        while (stack.Count > 0)
        {
            var (node, depth, isLast, indent) = stack.Pop();
            var prefix = isLast ? "└── " : "├── ";
            lines.Add($"{indent}{prefix}{labelFunc(node)}");

            var children = getChildren(node).Where(c => c != null).ToList();
            string childIndent = indent + (isLast ? "    " : "│   ");

            for (int i = children.Count - 1; i >= 0; i--)
            {
                bool isLastChild = i == children.Count - 1;
                stack.Push((children[i]!, depth + 1, isLastChild, childIndent));
            }
        }

        return string.Join("\n", lines);
    }
}

// Example usage with depth inspector style traversal
public static class ExampleHierarchy
{
    public class SomeThingToGenerate
    {
        public SomeComponent? MyComponent { get; set; }
        public SomeChildToGenerate? MyChild { get; set; }
        public int Fire { get; set; }
    }

    public class SomeChildToGenerate
    {
        public SomeComponent? MyComponent { get; set; }
        public int WalkWithMe { get; set; }
    }

    public class SomeComponent
    {
        public int TheAnswer { get; set; }
    }

    public static IEnumerable<(string Path, int Depth)> InspectDepths(object root)
    {
        var queue = new Queue<(object Node, string Path, int Depth)>();
        queue.Enqueue((root, "Root", 1));

        while (queue.Count > 0)
        {
            var (node, path, depth) = queue.Dequeue();
            yield return (path, depth);

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
