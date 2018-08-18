using System;
using System.Collections.Generic;
using System.Linq;
using Model.System;

namespace SOPS.Services.Templates
{
    public interface ITemplatesProvider
    {
        IList<Template> GetAllTemplates();
        Template GetTemplate(int id);
        Template GetTemplate(string templateName);
    }
}
