using System;
using System.Data;
using NHibernate;

namespace NHibernateRepository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;
        public ISession Session { get; private set; }

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            Session = _sessionFactory.OpenSession();
            Session.FlushMode = FlushMode.Auto;
        }        

        public void BeginTransaction()
        {
            if (_transaction != null && _transaction.IsActive == true)
            {
                throw new InvalidOperationException("Transaction is already active");
            }

            _transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted); 
        }

        public void Commit()
        {
            if (!_transaction.IsActive)
            {
                throw new InvalidOperationException("Transaction must be activated before commit");
            }

            _transaction.Commit();
            _transaction.Dispose();
        }

        public void Rollback()
        {
            if (_transaction.IsActive)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
        }

        public void Dispose()
        {
            if (Session.IsOpen)
            {
                Session.Close();
            }
        }
    }
}