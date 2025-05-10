using QuickAcid;
using QuickAcid.Bolts;
using QuickAcid.Bolts.Nuts;
using QuickMGenerate;
using QuickMGenerate.UnderTheHood;
using QuickPulse;

namespace QuickMGenerate.Tests._Tools;

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
        var run =
            from inspector in "inspector".Stashed(
                () =>
                {
                    var artery = new DistinctValueInspector<T>();
                    InspectContext.Current = a => artery.Flow(a);
                    return artery;
                })
            from input in "Generator".Input(generator.Inspect())
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
            from input in "Generator".Input(generator.Inspect())
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