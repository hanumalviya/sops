using NHMembership.Models;
using NHibernate;
using NHibernateRepository.Repository;

namespace NHMembership.DataAccess.Users
{
    public class UsersRepository : PersistRepository<int, UserProfile>, IUsersRepository
    {
        public UsersRepository(ISession session) :
            base(session)
        {

        }
    }
}