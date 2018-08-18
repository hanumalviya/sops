using Model.Companies;
using NHibernate;
using NHibernateRepository.Repository;
using System;
using System.Linq;
using SOPS.Repositories.Abstract;

namespace SOPS.Repositories.Implementation
{
    public class CompanyRepository : PersistRepository<int, Company>, ICompanyRepository
    {
        public CompanyRepository(ISession session) :
            base(session)
        {

        }
    }
}
