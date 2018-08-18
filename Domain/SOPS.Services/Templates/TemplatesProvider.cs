using System;
using System.Collections.Generic;
using System.Linq;
using Model.System;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;

namespace SOPS.Services.Templates
{
    public class TemplatesProvider : ITemplatesProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public TemplatesProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Template> GetAllTemplates()
        {
			var repository = _repositoriesFactory.CreateTemplateRepository(_unitOfWork);
			return repository.All().ToList();
        }

        public Template GetTemplate(int id)
        {
			var repository = _repositoriesFactory.CreateTemplateRepository(_unitOfWork);
			return repository.FindBy(id);
        }

        public Template GetTemplate(string templateName)
        {
            var repository = _repositoriesFactory.CreateTemplateRepository(_unitOfWork);
            return repository.FindBy(n => n.Name == templateName);
        }
    }
}
