using NHibernate;
using System.Collections.Generic;

namespace NHibernateRepository.Repository
{
    public class PersistRepository<TKey, TEntity> : ReadOnlyRepository<TKey, TEntity>, IPersistRepository<TEntity> where TEntity : class
    {
        public PersistRepository(ISession session) 
            : base(session)
        {

        }

        public void Add(TEntity entity)
        {
            _session.Save(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _session.Save(entity);
            }
        }

        public void Update(TEntity entity)
        {
            _session.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _session.Delete(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                _session.Delete(entity);
            }
        }
    }
}
