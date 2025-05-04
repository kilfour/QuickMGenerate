using QuickAcid;
using QuickAcid.Bolts;
using QuickAcid.Bolts.Nuts;
using QuickMGenerate;
using QuickMGenerate.Diagnostics;
using QuickMGenerate.Diagnostics.Inspectors;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.Tests._Tools;

public static class QA
{
    public static QAcidRunner<Acid> Should<TContainer, T>(
        Generator<T> generator,
        Func<Container<TContainer>> ctor,
        Action<Container<TContainer>, T> act,
        Func<Container<TContainer>, bool> testifyProven,
        Func<Container<TContainer>, QAcidRunner<Acid>> specs)
    {
        return
            from container in "container".Stashed(ctor)
            from input in "input".DynamicInput(generator)
            from _a in "act".Act(() => act(container, input))
            from _e in "early exit".TestifyProvenWhen(() => testifyProven(container))
            from _s in specs(container)
            select Acid.Test;
    }

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
}
public static class CheckIf
{
    public static (string, Func<T, bool>) Is<T>(T expected) =>
        (expected?.ToString() ?? "null", x => EqualityComparer<T>.Default.Equals(x, expected));

    public static void TheseValuesAreGenerated<T>(Generator<T> generator, params T[] needsToBeSeen)
    {
        GeneratedValuesShouldEventuallySatisfyAll(generator, [.. needsToBeSeen.Select(Is)]);
    }

    public static void GeneratesNullAndNotNull<T>(Generator<T> generator)
    {
        GeneratedValuesShouldEventuallySatisfyAll(generator,
            ("is null", a => a == null), ("is not null", a => a != null));
    }

    public static void GeneratedValuesShouldEventuallySatisfyAll<T>(
        Generator<T> generator,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        GeneratedValuesShouldEventuallySatisfyAll(100, generator, labeledChecks);
    }

    public static void GeneratedValuesShouldEventuallySatisfyAll<T>(
        int numberOfExecutions,
        Generator<T> generator,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        static DistinctValueInspector<T> inspectorFactory() => new();
        var run =
            from inspector in "inspector".Stashed(
                () => InspectorContext.SetCurrent(inspectorFactory()))
            from input in "Generator".Shrinkable(generator.Inspect())
            from _e in "early exit".TestifyProvenWhen(
                () => inspector.SeenSatisfyEach([.. labeledChecks.Select(a => a.Item2)]))
            from _s in "Assayer".Assay(
                [.. labeledChecks.Select(a => (a.Item1, (Func<bool>)(() => inspector.HasValueThatSatisfies(a.Item2))))])
            select Acid.Test;
        new QState(run).Testify(numberOfExecutions);
    }

    public static void GeneratedValuesShouldAllSatisfy<T>(
        Generator<T> generator,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        GeneratedValuesShouldAllSatisfy(20, generator, labeledChecks);
    }

    public static void GeneratedValuesShouldAllSatisfy<T>(
        int numberOfExecutions,
        Generator<T> generator,
        params (string, Func<T, bool>)[] labeledChecks)
    {
        var run =
            from input in "Generator".Shrinkable(generator.Inspect())
            from _ in CombineSpecs(input, labeledChecks) // Move this to QuickAcid
            select Acid.Test;
        new QState(run).Testify(numberOfExecutions);
    }

    private static QAcidRunner<Acid> CombineSpecs<T>(T input, IEnumerable<(string, Func<T, bool>)> checks)
    {
        return checks
            .Select(c => c.Item1.Spec(() => c.Item2(input)))
            .Aggregate(Acc, (acc, next) => from _ in acc from __ in next select Acid.Test);
    }

    public static readonly QAcidRunner<Acid> Acc =
        s => QAcidResult.AcidOnly(s);
}