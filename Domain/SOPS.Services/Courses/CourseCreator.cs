using System;
using System.Linq;
using Model.University;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;

namespace SOPS.Services.Courses
{
    public class CourseCreator : ICourseCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CourseCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public Course Create(string name, int departmentId)
        {
            var deparmentRepository = _repositoriesFactory.CreateDepartmentRepository(_unitOfWork);
            var department = deparmentRepository.FindBy(departmentId);
            return this.Create(name, department);
        }

        public Course Create(string name, Department department)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var courseRepository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
                var templatesRepository = _repositoriesFactory.CreateTemplateRepository(_unitOfWork);

                var template = templatesRepository.All().FirstOrDefault();
                var course = new Course() { Name = name, Department = department, Template = template };
                courseRepository.Add(course);              
                _unitOfWork.Commit();
                department.Courses.Add(course);

                return course;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

    }
}
