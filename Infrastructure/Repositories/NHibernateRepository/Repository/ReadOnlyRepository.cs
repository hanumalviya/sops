using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace NHibernateRepository.Repository
{
    public class ReadOnlyRepository<TKey, TEntity> : IReadOnlyRepository<TKey, TEntity> where TEntity : class
    {
        protected readonly ISession _session;

        public ReadOnlyRepository(ISession session)
        {
            _session = session;
        }

        public IQueryable<TEntity> All()
        {
            return _session.Query<TEntity>();
        }

        public TEntity FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return FilterBy(expression).SingleOrDefault();
        }

        public IQueryable<TEntity> FilterBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        public TEntity FindBy(TKey id)
        {
            return _session.Get<TEntity>(id);
        }
    }
}
