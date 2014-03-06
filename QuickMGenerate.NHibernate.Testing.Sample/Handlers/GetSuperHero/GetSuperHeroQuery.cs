using System;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using QuickMGenerate.NHibernate.Testing.Sample.Domain;

namespace QuickMGenerate.NHibernate.Testing.Sample.Handlers.GetSuperHero
{
    public class GetSuperHeroQuery
    {
        private readonly ISession session;

        public GetSuperHeroQuery(ISession session)
        {
            this.session = session;
        }

        public SuperHero One(Guid superHeroId)
        {
            return
                session
                    .CreateCriteria<SuperHero>()
                    .Add(Restrictions.Eq("Id", superHeroId))
                    .CreateAlias("SuperPowers", "sp", JoinType.LeftOuterJoin)
                    .CreateAlias("sp.SuperPowerEffects", "spe", JoinType.LeftOuterJoin)
                    .UniqueResult<SuperHero>();
        }
    }
}
