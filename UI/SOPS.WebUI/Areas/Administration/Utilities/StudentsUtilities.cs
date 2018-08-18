using System;
using System.Linq;
using System.Web.Mvc;
using Model.Students;
using SOPS.WebUI.Areas.Administration.ViewModels.Students;
using SOPS.Services.Courses;

namespace SOPS.WebUI.Areas.Administration.Utilities
{
    internal static class StudentsUtilities
    {
        public static StudentViewModel ToViewModel(this Student student)
        {
            var course = student.Course;
            if (course == null)
                return null;

            return new StudentViewModel()
            {
                Album = student.Album,
                City = student.City,
                Course = course.Name,
                Department = course.Department.Name,
                Email = student.Email,
                Id = student.Id,
                LastName = student.LastName,
                Mode = student.Mode.Id,
                Name = student.FirstName,
                Phone = student.Phone,
                PostalCode = student.PostalCode,
                Street = student.Address
            };
        }

        public static Student ToModel(this StudentViewModel student)
        {
            var coursesProvider = DependencyResolver.Current.GetService<ICoursesProvider>();
            var modesProvider = DependencyResolver.Current.GetService<IModesProvider>();

            var course = coursesProvider.GetCourses().SingleOrDefault(n => n.Name == student.Course);
            if (course == null)
                return null;

            return new Student()
            {
                Address = student.Street,
                Album = student.Album,
                City = student.City,
                Course = course,
                Email = student.Email,
                FirstName = student.Name,
                LastName = student.LastName,
                Mode = modesProvider.GetModes().SingleOrDefault(n => n.Id ==  student.Mode),
                Phone = student.Phone,
                PostalCode = student.PostalCode
            };
        }

        public static ContractViewModel ToViewModel(this Contract contract)
        {
            return contract == null ? null : new ContractViewModel()
            {
                CompanyId = contract.Company != null? contract.Company.Id : (int?)null,
                CompanyRepresentative = contract.CompanyRepresentative,
                EndDate = contract.EndDate,
                Id = contract.Id,
                StartDate = contract.StartDate,
                UniversityRepresentative = contract.UniversityRepresentative
            };
        }
    }
}