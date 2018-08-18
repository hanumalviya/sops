using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernateRepository.UnitOfWork;
using NHMembership.DataAccess.Users;

namespace NHMembership.DataAccess.Roles
{
    public interface IRolesRepositoryFactory
    {
        IRolesRepository CreateRolesRepository(IUnitOfWork unitOfWork);
    }
}
