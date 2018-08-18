using NHibernateRepository.Repository;
using NHMembership.Models;

namespace NHMembership.DataAccess.Users
{
    public interface IUsersRepository : IPersistRepository<UserProfile>, IReadOnlyRepository<int, UserProfile>
    {
    }
}
