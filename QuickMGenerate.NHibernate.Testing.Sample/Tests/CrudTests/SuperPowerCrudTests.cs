using System;
using QuickMGenerate.NHibernate.Testing.Sample.Domain;
using QuickMGenerate.NHibernate.Testing.Sample.Tests.Tools;
using QuickMGenerate.NHibernate.Testing.Sample.Tests.Tools;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.NHibernate.Testing.Sample.Tests.CrudTests
{
    public class SuperPowerCrudTests : CrudTest<SuperPower>
    {
		//protected override IDomainGenerator GenerateIt()
		//{
		//    return
		//        base.GenerateIt()
		//            .ManyToOne<SuperPower, SuperHero>(1, (sp, sh) => sh.SuperPowers.Add(sp))
		//            .ForEach<SuperHero>(e => NHibernateSession.Save(e));
		//}

    	protected override Generator<SuperPower> GenerateIt()
    	{
    		throw new NotImplementedException();
    	}

    	protected override bool DeleteEntity(SuperPower entity)
        {
            // Why ? : 
            //  - deleted when the last SuperHero with this superpower dies.
            //  - unidirectional relation means we need a more specific test.
            return false; 
        }

        [Fact]
        public void HasManySuperPowers()
        {
            HasMany(e => e.SuperPowerEffects);
        }
    }
}