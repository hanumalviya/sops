using System;
using System.Linq;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;

namespace SOPS.Services.Templates
{
    public class TemplateDestructor : ITemplateDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public TemplateDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Destroy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateTemplateRepository(_unitOfWork);
                var template = repository.FindBy(id);
                repository.Delete(template);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public bool CanBeDestroyed(int id)
        {            
            var repository = _repositoriesFactory.CreateTemplateRepository(_unitOfWork);
            var template = repository.FindBy(id);

            return template.Courses.Any() == false;
        }
    }
}
