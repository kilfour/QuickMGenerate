using QuickAcid;
using QuickAcid.Bolts;
using QuickAcid.Bolts.Nuts;
using QuickMGenerate;
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
            from input in "input".Derived(generator)
            from _a in "act".Act(() => act(container, input))
            from _e in "early exit".TestifyProvenWhen(() => testifyProven(container))
            from _s in specs(container)
            select Acid.Test;
    }
}
