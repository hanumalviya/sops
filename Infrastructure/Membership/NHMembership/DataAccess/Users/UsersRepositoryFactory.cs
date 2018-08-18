using System;
using System.Linq;

namespace NHMembership.DataAccess.Users
{
    public class UsersRepositoryFactory : IUsersRepositoryFactory
    {
        public IUsersRepository CreateUsersRepository(NHibernateRepository.UnitOfWork.IUnitOfWork unitOfWork)
        {
            return new UsersRepository(unitOfWork.Session);
        }
    }
}
