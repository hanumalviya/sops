using Model.Companies;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Companies
{
    public class CompanyCreator : ICompanyCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public Company Create(
            string name,
            string street,
            string city,
            string postalCode,
            string email,
            string phone,
            string url,
            string description)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateCompanyRepository(_unitOfWork);
                var company = new Company()
                {
                    Name = name,
                    Street = street,
                    City = city,
                    PostalCode = postalCode,
                    Email = email,
                    Phone = phone,
                    Url = url,
                    Description = description
                };

                repository.Add(company);
                _unitOfWork.Commit();

                return company;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
