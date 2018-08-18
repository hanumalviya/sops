using Model.Students;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Abstract;
using SOPS.Repositories.Factory;
using SOPS.Repositories.Implementation;
using System;
using System.Linq;

namespace SOPS.Services.Students
{
    public class StudentDestructor : IStudentDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public StudentDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Destroy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateStudentRepository(_unitOfWork);
                var student = repository.FindBy(id);
                repository.Delete(student);
                student.Course.Students.Remove(student);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public void DestroyAllStudents()
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var repository = _repositoriesFactory.CreateStudentRepository(_unitOfWork);
                var allStudents = repository.All().ToList();

                foreach (var item in allStudents)
                {
                    repository.Delete(item);
                    item.Course.Students.Remove(item);
                }

                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
