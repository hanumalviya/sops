using Model.Employees;
using SOPS.Services.Courses;
using SOPS.WebUI.Areas.Administration.ViewModels.Employees;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.Utilities
{
    public static class EmployeesUtilities
    {
        public static EmployeeViewModel ToViewModel(this Employee e)
        {
            return e == null ? null : new EmployeeViewModel()
            {
                Administrator = e.Administrator,
                Course = e.Course.Id,
                Department = e.Course.Department.Id,
                Email = e.Email,
                FirstName = e.FirstName,
                Id = e.Id,
                LastName = e.LastName,
                Moderator = e.Moderator,
                SuperAdministrator = e.Root
            };
        }

        public static Employee ToModel(this EmployeeViewModel e)
        {
            var coursesProvider = DependencyResolver.Current.GetService<ICoursesProvider>();
            var course = coursesProvider.GetCourse(e.Course);

            if (e != null)
            {
                var employee = new Employee()
                {
                    Administrator = e.Administrator,
                    Email = e.Email,
                    FirstName = e.FirstName,
                    Id = e.Id,
                    LastName = e.LastName,
                    Moderator = e.Moderator,
                    Root = e.SuperAdministrator,
                    Course = course
                };

                return employee;
            }

            return null;
        }
    }
}