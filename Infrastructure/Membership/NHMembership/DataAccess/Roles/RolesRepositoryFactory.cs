namespace NHMembership.DataAccess.Roles
{
    public class RolesRepositoryFactory : IRolesRepositoryFactory
    {
        public IRolesRepository CreateRolesRepository(NHibernateRepository.UnitOfWork.IUnitOfWork unitOfWork)
        {
            return new RolesRepository(unitOfWork.Session);
        }
    }
}
