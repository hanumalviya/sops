using Model.System;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface ITemplateRepository : IPersistRepository<Template>, IReadOnlyRepository<int, Template>
    {
    }
}
