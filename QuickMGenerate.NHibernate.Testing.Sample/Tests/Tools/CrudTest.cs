using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Exceptions;
using QuickMGenerate.NHibernate.Testing.Sample.Domain;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickMGenerate.NHibernate.Testing.Sample.Tests.Tools
{
    public abstract class CrudTest<TEntity> : DatabaseTest
        where TEntity : class, IHaveAnId
    {
    	protected abstract Generator<State, TEntity> GenerateIt();

        protected void IsRequired<T>(Expression<Func<TEntity, T>> expression)
        {
            Assert.Throws<GenericADOException>(
                () =>
                	{
                		(from _ in MGen.For<TEntity>().Ignore(expression)
                		 from entity in GenerateIt()
                		 select entity)
                			.Generate();
                        NHibernateSession.Flush();
                    });
        }

        protected void HasA<T>(Expression<Func<TEntity, T>> expression)
            where T : IHaveAnId
        {
            var compiledExpression = expression.Compile();
            var entity = BuildEntity();
            NHibernateSession.Flush();
            var entityId = entity.Id;
            var childId = compiledExpression.Invoke(entity).Id;
            NHibernateSession.Clear();
            entity = NHibernateSession.Get<TEntity>(entityId);
            Assert.Equal(childId, compiledExpression.Invoke(entity).Id);
        }

        protected void HasMany<TMany>(Expression<Func<TEntity, IEnumerable<TMany>>> expression)
            where TMany : IHaveAnId
        {
            var compiledExpression = expression.Compile();
            var entity = BuildEntity();
            NHibernateSession.Flush();
            var manies = compiledExpression.Invoke(entity).ToList();
            Assert.NotEqual(0, manies.Count());
            var entityId = entity.Id;
            var ids = manies.Select(many => many.Id).ToList();
            NHibernateSession.Clear();
            entity = NHibernateSession.Get<TEntity>(entityId);
            manies = compiledExpression.Invoke(entity).ToList();
            Assert.Equal(ids.Count, manies.Count());
            foreach (var many in manies)
            {
                Assert.True(ids.Contains(many.Id));
            }
        }

        [Fact]
        public void SelectQueryWorks()
        {
            NHibernateSession.CreateCriteria(typeof (TEntity)).SetMaxResults(5).List();
        }

        [Fact]
        public void AddEntityEntityWasAdded()
        {
            var entity = BuildEntity();
            NHibernateSession.Flush();
            NHibernateSession.Evict(entity);
            var reloadedEntity = NHibernateSession.Get<TEntity>(entity.Id);
            Assert.NotNull(reloadedEntity);
            AssertEqual(entity, reloadedEntity);
            Assert.NotEqual(Guid.Empty, entity.Id);
        }

        [Fact]
        public void UpdateEntityEntityWasUpdated()
        {
            var entity = BuildEntity();
            NHibernateSession.Flush();
            ModifyEntity(entity);
            UpdateEntity(entity);
            NHibernateSession.Evict(entity);
            var reloadedEntity = NHibernateSession.Get<TEntity>(entity.Id);
            Assert.NotNull(reloadedEntity);
            AssertEqual(entity, reloadedEntity);
        }

        [Fact]
        public void DeleteEntityEntityWasDeleted()
        {
            var entity = BuildEntity();
            NHibernateSession.Flush();
            if (DeleteEntity(entity))
            {
                NHibernateSession.Flush();
                Assert.Null(NHibernateSession.Get<TEntity>(entity.Id));
            }
        }

        private static void AssertEqual<T>(T expectedEntity, T actualEntity)
        {
            Inspect
                .This(expectedEntity, actualEntity)
                .Report(Assert.True)
                .AreMemberWiseEqual();
        }

        private TEntity BuildEntity()
        {
            var generator =
				from _ in MGen.For<IHaveAnId>().Ignore(e => e.Id)
				from entity in GenerateIt()
				select entity;
            return generator.Generate();
        }

        private void ModifyEntity(TEntity entity)
        {
			var generator =
				from _ in MGen.For<IHaveAnId>().Ignore(e => e.Id)
				from e in GenerateIt()
				select e;
        	generator.Modify(entity).Generate();
        }

        private void UpdateEntity(TEntity entity)
        {
            NHibernateSession.Update(entity);
            NHibernateSession.Flush();
        }

        protected virtual bool DeleteEntity(TEntity entity)
        {
            NHibernateSession.Delete(entity);
            return true;
        }
    }
}