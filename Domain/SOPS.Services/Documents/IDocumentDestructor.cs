using System;
using System.Linq;

namespace SOPS.Services.Documents
{
    public interface IDocumentDestructor
    {
        void Destroy(int id);
    }
}
