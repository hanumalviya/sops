using System;
using System.Linq;

namespace SOPS.Services.Templates
{
    public interface ITemplateDestructor
    {
        void Destroy(int id);
        bool CanBeDestroyed(int id);
    }
}
