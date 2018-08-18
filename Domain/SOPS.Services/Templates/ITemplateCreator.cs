using Model.System;
using System;
using System.Linq;

namespace SOPS.Services.Templates
{
    public interface ITemplateCreator
    {
        Template Create(string fileName, string filePath, string ContentType);
    }
}
