using System;
using System.Linq;

namespace SOPS.Services.Companies
{
    public interface ICompanyDestructor
    {
        void Destroy(int id);

        bool CanBeDestroy(int id);
    }
}
