using NHibernate;
using System;
using System.Linq;
using Model.Students;
using SOPS.Repositories.Abstract;

namespace SOPS.Repositories.Implementation
{
    public class ContractRepository : NHibernateRepository.Repository.PersistRepository<int, Contract>, IContractRepository
    {
        public ContractRepository(ISession session) :
            base(session)
        {

        }
    }
}
