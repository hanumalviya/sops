using System;
using System.Linq;
using Model.University;
using SOPS.Repositories.Factory;
using NHibernateRepository.UnitOfWork;
using NHMembership.Services;
using Model.Employees;

namespace SOPS.Services.Courses
{
    public  class CourseUpdater : ICourseUpdater
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public CourseUpdater(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory, IAuthenticationService authenticationService)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        public void Update(Course course)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var courseRepository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
                 courseRepository.Update(course);             
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        public void SetManager(Course course, Employee manager)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var employeeRepository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
                var courseRepository = _repositoriesFactory.CreateCourseRepository(_unitOfWork);
                var actualCourse = courseRepository.FindBy(course.Id);

                if (actualCourse.Manager != null)
                {
                    actualCourse.Manager.Keeper = false;
                    employeeRepository.Update(actualCourse.Manager);
                    if (_authenticationService.RoleService.IsUserInRole(actualCourse.Manager.UserName, EmployeesRoles.Keeper))
                        _authenticationService.RoleService.RemoveUserFromRole(actualCourse.Manager.UserName, EmployeesRoles.Keeper);
                }

                if (manager != null)
                {
                    course.Manager = manager;
                    course.Manager.Keeper = true;

                    employeeRepository.Update(course.Manager);
                    if (_authenticationService.RoleService.IsUserInRole(manager.UserName, EmployeesRoles.Keeper) == false)
                        _authenticationService.RoleService.AddUserToRole(manager.UserName, EmployeesRoles.Keeper);
                }
                else
                {
                    course.Manager = null;
                }

                courseRepository.Update(course);
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
