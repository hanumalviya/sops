using NHibernateRepository.UnitOfWork;

namespace NHMembership.DataAccess.Users
{
    public interface IUsersRepositoryFactory
    {
        IUsersRepository CreateUsersRepository(IUnitOfWork unitOfWork);
    }
}
