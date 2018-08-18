using System;
using System.IO;
using System.Linq;

namespace SOPS.Services.Documents
{
    public interface IDocumentCreator
    {
        void Create(string fileName, string filePath, string ContentType);
    }
}
