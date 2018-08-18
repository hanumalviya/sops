using Model.System;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class UniversityRepository : PersistRepository<int, University>, IUniversityRepository
    {
        public UniversityRepository(ISession session) :
            base(session)
        {

        }
    }
}
