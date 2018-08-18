using Model.Companies;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface ICompanyRepository : IPersistRepository<Company>, IReadOnlyRepository<int, Company>
    {
    }
}
