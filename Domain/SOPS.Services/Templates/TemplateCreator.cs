using System;
using System.Linq;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using Model.System;

namespace SOPS.Services.Templates
{
    public class TemplateCreator : ITemplateCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public TemplateCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public Template Create(string fileName, string filePath, string contentType)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateTemplateRepository(_unitOfWork);
                Template template = new Template() { Name = fileName, FilePath = filePath, ContentType = contentType };
                repository.Add(template);
                _unitOfWork.Commit();

                return template;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
