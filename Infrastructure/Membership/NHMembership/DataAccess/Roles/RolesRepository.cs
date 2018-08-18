using NHMembership.Models;
using NHibernate;
using NHibernateRepository.Repository;

namespace NHMembership.DataAccess.Roles
{
    public class RolesRepository : PersistRepository<int, Role>, IRolesRepository
    {
        public RolesRepository(ISession session) :
            base(session)
        {
        }
    }
}