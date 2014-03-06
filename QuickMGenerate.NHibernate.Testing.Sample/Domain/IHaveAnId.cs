using System;

namespace QuickMGenerate.NHibernate.Testing.Sample.Domain
{
    public interface IHaveAnId
    {
        Guid Id { get; }
    }
}