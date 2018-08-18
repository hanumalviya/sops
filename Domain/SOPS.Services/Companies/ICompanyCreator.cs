using Model.Companies;
using System;
using System.Linq;

namespace SOPS.Services.Companies
{
    public interface ICompanyCreator
    {
        Company Create(
            string name,
            string street,
            string city,
            string postalCode,
            string email,
            string phone,
            string url,
            string description);
    }
}
