using System;
using System.Collections.Generic;
using System.Linq;
using Model.Companies;
using SOPS.Services.Utilities;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;

namespace SOPS.Services.Companies
{
    public class CompaniesProvider : ICompaniesProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }
        
        public IList<Company> GetCompanies(string filter)
        {
            var repository = _repositoriesFactory.CreateCompanyRepository(_unitOfWork);

			var results = repository.All().ToList().Where (n => n.Name.InsensitiveContains(filter) ||
				n.Email.InsensitiveContains(filter) ||
				n.Description.InsensitiveContains(filter) ||
				n.Street.InsensitiveContains(filter)).ToList();

			return results;
        }

        public Company GetCompany(int id)
        {
            var repository = _repositoriesFactory.CreateCompanyRepository(_unitOfWork);
			return repository.FindBy(id);
        }
    }
}
