using QuickMGenerate;

namespace QuickMGenerate.Diagnostics.Inspectors;


public static class Depth
{
    public static DepthTracker Track()
    {
        return new DepthTracker();
    }

    public class DepthTracker : Inspector
    {
        public List<List<(string Path, string type, int Depth)>> Depths { get; private set; } = [];

        public void Log(string[] tags, string message, object data)
        {
            Depths.Add([.. InspectDepths(data)]);
        }

        private static IEnumerable<(string Path, string type, int Depth)> InspectDepths(object root)
        {
            var queue = new Queue<(object Node, string Path, int Depth)>();
            queue.Enqueue((root, "Root", 1));

            while (queue.Count > 0)
            {
                var (node, path, depth) = queue.Dequeue();
                yield return (path, node.GetType().Name, depth);

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
}