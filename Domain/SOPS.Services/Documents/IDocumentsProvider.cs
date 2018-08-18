using System;
using System.Collections.Generic;
using System.Linq;
using Model.System;

namespace SOPS.Services.Documents
{
    public interface IDocumentsProvider
    {
        IList<Document> GetAllDocuments();
        Document GetDocument(int id);
    }
}
