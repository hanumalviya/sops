using Model.System;
using NHibernate;
using NHibernateRepository.Repository;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Implementation
{
    public class DocumentRepository : PersistRepository<int, Document>, IDocumentRepository
    {
        public DocumentRepository(ISession session) :
            base(session)
        {

        }
    }
}
