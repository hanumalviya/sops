using Model.System;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class TemplateRepository : PersistRepository<int, Template>, ITemplateRepository
    {
        public TemplateRepository(ISession session) :
            base(session)
        {

        }
    }
}
