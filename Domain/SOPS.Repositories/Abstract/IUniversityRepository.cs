using Model.System;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface IUniversityRepository : IPersistRepository<University>, IReadOnlyRepository<int, University>
    {
    }
}
