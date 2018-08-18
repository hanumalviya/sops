using Model.Companies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Companies
{
    public interface ICompaniesProvider
    {
        IList<Company> GetCompanies(string filter);
        Company GetCompany(int id);
    }
}
