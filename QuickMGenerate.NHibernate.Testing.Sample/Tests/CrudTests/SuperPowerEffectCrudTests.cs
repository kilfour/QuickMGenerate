using System;
using QuickMGenerate.NHibernate.Testing.Sample.Domain;
using QuickMGenerate.NHibernate.Testing.Sample.Tests.Tools;
using QuickMGenerate.NHibernate.Testing.Sample.Tests.Tools;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate.NHibernate.Testing.Sample.Tests.CrudTests
{
    public class SuperPowerEffectCrudTests : CrudTest<SuperPowerEffect>
    {
		//protected override IDomainGenerator GenerateIt()
		//{
		//    return
		//        base.GenerateIt()
		//            .ManyToOne<SuperPower, SuperHero>(1, (sp, sh) => sh.SuperPowers.Add(sp))
		//            .ManyToOne<SuperPowerEffect, SuperPower>(1, (spe, sp) => sp.SuperPowerEffects.Add(spe))
		//            .ForEach<SuperHero>(e => NHibernateSession.Save(e));
		//}

    	protected override Generator<State, SuperPowerEffect> GenerateIt()
    	{
    		throw new NotImplementedException();
    	}

    	protected override bool DeleteEntity(SuperPowerEffect entity)
        {
            // Why ? : 
            //  - deleted when the last SuperHero with this superpower dies.
            //  - unidirectional relation means we need a more specific test.
            return false;
        }
    }
}