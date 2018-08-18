using Model.University;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class ModeRepository : PersistRepository<int, Mode>, IModeRepository
    {
        public ModeRepository(ISession session) :
            base(session)
        {

        }
    }
}
