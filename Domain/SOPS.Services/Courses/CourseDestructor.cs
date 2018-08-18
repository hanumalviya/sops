using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Courses
{
    public class CourseDestructor : ICourseDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CourseDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public void Destroy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
                var course = repository.FindBy(id);

                course.Department.Courses.Remove(course);

                repository.Delete(course);
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
            var courseRepository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
            var employeesRepository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
            var isUsed = employeesRepository.All().Any(n => n.Course != null && n.Course.Id == id);
            var course = courseRepository.FindBy(id);
            return course.Students.Any() == false && course.Manager == null && isUsed == false;
        }
    }
}
