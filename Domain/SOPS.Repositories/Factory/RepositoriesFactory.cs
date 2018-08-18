using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Abstract;
using SOPS.Repositories.Implementation;
using System;
using System.Linq;

namespace SOPS.Repositories.Factory
{
    public class RepositoriesFactory : IRepositoriesFactory
    {
        public ICompanyRepository CreateCompanyRepository(IUnitOfWork unitOfWork)
        {
            return new CompanyRepository(unitOfWork.Session);
        }

        public IContractRepository CreateContractRepository(IUnitOfWork unitOfWork)
        {
            return new ContractRepository(unitOfWork.Session);
        }

        public ICourseRepository CreateCourseRepository(IUnitOfWork unitOfWork)
        {
            return new CourseRepository(unitOfWork.Session);
        }

        public IDepartmentRepository CreateDepartmentRepository(IUnitOfWork unitOfWork)
        {
            return new DepartmentRepository(unitOfWork.Session);
        }

        public IDocumentRepository CreateDocumentRepository(IUnitOfWork unitOfWork)
        {
            return new DocumentRepository(unitOfWork.Session);
        }

        public IEmployeeRepository CreateEmployeeRepository(IUnitOfWork unitOfWork)
        {
            return new EmployeeRepository(unitOfWork.Session);
        }

        public IModeRepository CreateModeRepository(IUnitOfWork unitOfWork)
        {
            return new ModeRepository(unitOfWork.Session);
        }

        public IOfferRepository CreateOfferRepository(IUnitOfWork unitOfWork)
        {
            return new OfferRepository(unitOfWork.Session);
        }

        public IOfferTypeRepository CreateOfferTypeRepository(IUnitOfWork unitOfWork)
        {
            return new OfferTypeRepository(unitOfWork.Session);
        }

        public IStudentRepository CreateStudentRepository(IUnitOfWork unitOfWork)
        {
            return new StudentRepository(unitOfWork.Session);
        }

        public ITemplateRepository CreateTemplateRepository(IUnitOfWork unitOfWork)
        {
            return new TemplateRepository(unitOfWork.Session);
        }

        public IUniversityRepository CreateUniversityRepository(IUnitOfWork unitOfWork)
        {
            return new UniversityRepository(unitOfWork.Session);
        }
    }
}
