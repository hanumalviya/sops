using Model.University;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface IDepartmentRepository : IPersistRepository<Department>, IReadOnlyRepository<int, Department>
    {
    }
}
