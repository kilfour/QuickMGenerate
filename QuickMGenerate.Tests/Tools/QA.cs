using QuickAcid;
using QuickAcid.Bolts;
using QuickAcid.Bolts.Nuts;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests.Tools;

public static class QA
{
    public static QAcidRunner<Acid> Should<TContainer, T>(
        string label,
        Generator<T> generator,
        Func<Container<TContainer>> ctor,
        Action<Container<TContainer>, T> act,
        Func<Container<TContainer>, bool> spec)
    {
        return
            from container in "container".Stashed(ctor)
            from input in "input".DynamicInput(generator)
            from _a in "act".Act(() => act(container, input))
            from _e in "early exit".TestifyProvenWhen(() => spec(container))
            from _s in label.Assay(() => spec(container))
            select Acid.Test;
    }

    public static QAcidRunner<Acid> ShouldEventuallyBe<T>(
        string label,
        Generator<T> generator,
        Action<Container<T>, T> act,
        Func<Container<T>, bool> spec)
    {
        return Should(label, generator, () => new Container<T>(), act, spec);
    }

    public static QAcidRunner<Acid> ShouldEventuallyBeNullAndNotNull<T>(string label, Generator<T?> generator)
    {
        return Should($"{label}: null and non-null seen", generator, () => new Container<HashSet<string>>([])
            , (c, v) => c.Value!.Add(v is null ? "null" : "non-null")
            , c => c.Value!.Contains("null") && c.Value!.Contains("non-null"));
    }

    public static QAcidRunner<Acid> ShouldEventuallyBeNullAndNotNull<T, TProperty>(
        string label
        , Generator<T?> generator,
        Func<T, TProperty> getter)
    {
        return Should($"{label}: null and non-null seen", generator, () => new Container<HashSet<string>>([])
            , (c, v) => c.Value!.Add(getter(v!) is null ? "null" : "non-null")
            , c => c.Value!.Contains("null") && c.Value!.Contains("non-null"));
    }
}
