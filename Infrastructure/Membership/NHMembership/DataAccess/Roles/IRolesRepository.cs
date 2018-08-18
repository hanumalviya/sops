using NHibernateRepository.Repository;
using NHMembership.Models;

namespace NHMembership.DataAccess.Roles
{
    public interface IRolesRepository : IPersistRepository<Role>, IReadOnlyRepository<int, Role>
    {
    }
}
