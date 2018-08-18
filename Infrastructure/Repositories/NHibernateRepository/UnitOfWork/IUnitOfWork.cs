using NHibernate;
using System;

namespace NHibernateRepository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();

        ISession Session { get; }
    }
}