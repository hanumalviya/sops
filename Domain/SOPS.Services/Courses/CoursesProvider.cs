using Model.University;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPS.Services.Courses
{
    public class CoursesProvider : ICoursesProvider
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CoursesProvider(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public IList<Course> GetCourses()
        {
            var repository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
            return repository.All().ToList();
        }

        public Course GetCourse(int id)
        {            
            var repository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
            return repository.FindBy(id);
        }

        public Course GetCourse(string courseName)
        {
            var repository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
            return repository.FindBy(n => n.Name == courseName);
        }
    }
}
