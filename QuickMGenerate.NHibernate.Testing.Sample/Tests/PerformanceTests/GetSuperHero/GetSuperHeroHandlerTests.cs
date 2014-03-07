using QuickMGenerate.NHibernate.Testing.Sample.Domain;
using QuickMGenerate.NHibernate.Testing.Sample.Handlers.GetSuperHero;
using QuickMGenerate.NHibernate.Testing.Sample.Tests.Tools;
using QuickMGenerate.NHibernate.Testing.Sample.Domain;
using Xunit;

namespace QuickMGenerate.NHibernate.Testing.Sample.Tests.PerformanceTests.GetSuperHero
{
    public class GetSuperHeroHandlerTests : DatabaseTest
    {
        [Fact]
        public void DoesNotCauseLazyLoading()
        {
        	var generator =
        		from _ in MGen.For<IHaveAnId>().Ignore(e => e.Id)
        		from powers in MGen.One<SuperPower>().Many(5)
        		from hero in MGen.One<SuperHero>()
        			.Apply(e => SaveToSession(e))
					.Apply(h => powers.ForEach(p => h.SuperPowers.Add(p)) )
        		select hero;

			var superhero = generator.Generate();

            FlushAndClear();

            var id = superhero.Id;
            
            var handler = new GetSuperHeroHandler(new GetSuperHeroQuery(NHibernateSession));

            using(1.Queries())
            {
                handler.Handle(id);
            }
        }
    }
}
