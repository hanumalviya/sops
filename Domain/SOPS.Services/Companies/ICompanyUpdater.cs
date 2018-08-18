using Model.Companies;
using System;
using System.Linq;

namespace SOPS.Services.Companies
{
    public interface ICompanyUpdater
    {
        void Update(Company company);
    }
}
