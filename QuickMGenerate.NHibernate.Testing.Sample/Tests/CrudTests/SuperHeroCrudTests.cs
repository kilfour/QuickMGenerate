using QuickMGenerate.NHibernate.Testing.Sample.Domain;
using QuickMGenerate.NHibernate.Testing.Sample.Tests.Tools;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.NHibernate.Testing.Sample.Tests.CrudTests
{
    public class SuperHeroCrudTests : CrudTest<SuperHero>
    {
		protected override Generator<State, SuperHero> GenerateIt()
		{
			return
				from _ in MGen.For<IHaveAnId>().Ignore(e => e.Id)
				from powers in MGen.One<SuperPower>().Many(5)
				from hero in MGen.One<SuperHero>()
					.Apply(e => SaveToSession(e))
					.Apply(h => powers.ForEach(p => h.SuperPowers.Add(p)))
				select hero;
		}

        [Fact]
        public void HasManySuperPowers()
        {
            HasMany(e => e.SuperPowers);
        }
    }
}
