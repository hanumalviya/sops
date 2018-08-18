using System.Collections.Generic;

namespace NHibernateRepository.Repository
{
    public interface IPersistRepository<in TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
    }
}
