using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Abstract;
using System;
using System.Linq;

namespace SOPS.Repositories.Factory
{
    public interface IRepositoriesFactory
    {
        ICompanyRepository CreateCompanyRepository(IUnitOfWork unitOfWork);
        IContractRepository CreateContractRepository(IUnitOfWork unitOfWork);
        ICourseRepository CreateCourseRepository(IUnitOfWork unitOfWork);
        IDepartmentRepository CreateDepartmentRepository(IUnitOfWork unitOfWork);
        IDocumentRepository CreateDocumentRepository(IUnitOfWork unitOfWork);
        IEmployeeRepository CreateEmployeeRepository(IUnitOfWork unitOfWork);
        IModeRepository CreateModeRepository(IUnitOfWork unitOfWork);
        IOfferRepository CreateOfferRepository(IUnitOfWork unitOfWork);
        IOfferTypeRepository CreateOfferTypeRepository(IUnitOfWork unitOfWork);
        IStudentRepository CreateStudentRepository(IUnitOfWork unitOfWork);
        ITemplateRepository CreateTemplateRepository(IUnitOfWork unitOfWork);
        IUniversityRepository CreateUniversityRepository(IUnitOfWork unitOfWork);
    }
}
