using Model.University;
using SOPS.Services.Departments;
using SOPS.Services.Employees;
using SOPS.Services.Templates;
using SOPS.WebUI.Areas.Administration.ViewModels.University;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SOPS.WebUI.Areas.Administration.Utilities
{
    public static class UniversityUtilities
    {
        public static CourseViewModel ToViewModel(this Course c)
        {
            return c == null ? null : new CourseViewModel()
            {
                Id = c.Id,
                DepartmentId = c.Department.Id,
                Keeper = c.Manager != null ? c.Manager.Id : (int?)null,
                Name = c.Name,
                Template = c.Template != null ? c.Template.Id : (int?)null
            };
        }

        public static Course ToModel(this CourseViewModel c)
        {
            var emplooyeeProvider = DependencyResolver.Current.GetService<IEmployeesProvider>();
            var departmentProvider = DependencyResolver.Current.GetService<IDepartmentsProvider>();
            var templatesProvider = DependencyResolver.Current.GetService<ITemplatesProvider>();
            return c == null ? null : new Course()
            {
                Id = c.Id,
                Manager = c.Keeper.HasValue == true ?  emplooyeeProvider.GetEmployee(c.Keeper.Value) : null,
                Name = c.Name,
                Template = c.Template.HasValue == true ? templatesProvider.GetTemplate(c.Template.Value) : null,
                Department = departmentProvider.GetAllDepartments().Where(n => n.Courses.Any(x => x.Id == c.Id)).Single()
            };
        }


        public static DepartmentViewModel ToViewModel(this Department d)
        {
            return d == null ? null : new DepartmentViewModel()
            {
                Id = d.Id,
                Name = d.Name,
                
                Courses = d.Courses.Select(n => n.ToViewModel()).ToList()
            };
        }

        public static Department ToModel(this DepartmentViewModel d)
        {
            return d == null ? null : new Department()
            {
                Id = d.Id,
                Name = d.Name,
                //Courses = d.Courses.Select(n => n.ToModel()).ToList()
            };
        }
    }
}