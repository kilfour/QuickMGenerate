using System.Linq.Expressions;
using System.Reflection;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests._Tools;

public class HierarchyLabeler<T>
{
    public record LabeledProperty(char Label, Type Type, PropertyInfo PropertyInfo);

    private readonly List<LabeledProperty> labeledProperties = [];
    private readonly char rootLabel;
    private readonly char nullLabel;

    public HierarchyLabeler(char rootLabel, char nullLabel)
    {
        this.rootLabel = rootLabel;
        this.nullLabel = nullLabel;
    }

    public class LabelBuilder<TParent, TDerived, TProperty>
    {
        private readonly HierarchyLabeler<TParent> parent;

        private readonly PropertyInfo propertyInfo;

        public LabelBuilder(HierarchyLabeler<TParent> parent, PropertyInfo propertyInfo)
        {
            this.parent = parent;
            this.propertyInfo = propertyInfo;
        }

        public HierarchyLabeler<TParent> With(char label)
        {
            return parent.AddLabeledProperty(label, typeof(TDerived), propertyInfo);
        }

        // AndThen
    }

    public HierarchyLabeler<T> AddLabeledProperty(char label, Type type, PropertyInfo propertyInfo)
    {
        labeledProperties.Add(new(label, type, propertyInfo));
        return this;
    }

    public LabelBuilder<T, TDerived, TProperty> LabelFor<TDerived, TProperty>(Expression<Func<TDerived, TProperty>> property)
        where TDerived : T
    {
        return new LabelBuilder<T, TDerived, TProperty>(this, property.AsPropertyInfo());
    }

    public LabelBuilder<T, T, TProperty> Label<TProperty>(Expression<Func<T, TProperty>> property)
    {
        return new LabelBuilder<T, T, TProperty>(this, property.AsPropertyInfo());
    }

    public List<char[]> Probe(object parent)
    {
        List<char[]> Labels = [];
        var stack = new Stack<(object Node, char[] label)>();
        stack.Push((parent, [rootLabel]));
        Labels.Add([rootLabel]);
        while (stack.Count > 0)
        {
            var (node, label) = stack.Pop();

            var properties = labeledProperties.Where(a => a.Type == node.GetType());
            foreach (var labeledProperty in properties)
            {
                var child = labeledProperty.PropertyInfo.GetValue(node);
                if (child != null)
                {
                    var newLabel = Append(label, labeledProperty.Label);
                    Labels.Add(newLabel);
                    stack.Push((child, newLabel));
                }
                else
                {
                    var newLabel = Append(label, nullLabel);
                    Labels.Add(newLabel);
                }
            }
        }
        return Labels;
    }

    private static char[] Append(char[] source, char item)
    {
        var result = new char[source.Length + 1];
        Array.Copy(source, result, source.Length);
        result[source.Length] = item;
        return result;
    }
}
