using System;
using System.Collections.Generic;
using System.Linq;
using Model.Students;
using SOPS.Services.Utilities;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;

namespace SOPS.Services.Students
{
    public class StudentsProvider : IStudentsProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public StudentsProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Student> GetStudents(int courseId, string filter)
        {
            var repository = _repositoriesFactory.CreateStudentRepository(_unitOfWork);
            var results = repository.FilterBy(x => x.Course.Id == courseId).ToList()
                                    .Where (n => n.FirstName.InsensitiveContains(filter) ||
                                    n.LastName.InsensitiveContains(filter) ||
                                    n.Mode.Name.ToLower() == filter.ToLower() ||
                                    n.Album.InsensitiveContains(filter)).ToList();

            return results;
        }


        public IList<Student> GetStudents(int courseId)
        {
            var repository = _repositoriesFactory.CreateStudentRepository(_unitOfWork);
            return repository.All().ToList();
        }

        public Student GetStudent(int id)
        {            
            var repository = _repositoriesFactory.CreateStudentRepository(_unitOfWork);
            return repository.FindBy(id);
        }
    }
}
