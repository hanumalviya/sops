using Model.System;
using NHibernateRepository.Repository;
using System;
using System.Linq;

namespace SOPS.Repositories.Abstract
{
    public interface IDocumentRepository : IPersistRepository<Document>, IReadOnlyRepository<int, Document>
    {
    }
}
