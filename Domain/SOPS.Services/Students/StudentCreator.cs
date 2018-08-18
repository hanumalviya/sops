using Model.Students;
using Model.University;
using NHibernateRepository.UnitOfWork;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Students
{
    public class StudentCreator : IStudentCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;

        public StudentCreator(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
        }

        public Student Create(
            string firstName, 
            string lastName, 
            string album, 
            Course course, 
            Mode mode, 
            string email = "",
            string phone = "",
            string city = "",
            string address = "",
            string postalCode = "")
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateStudentRepository(_unitOfWork);
                var student = new Student()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Album = album,
                    Course = course,
                    Mode = mode,
                    Email = email,
                    Phone = phone,
                    City = city,
                    Address = address,
                    PostalCode = postalCode
                };

                repository.Add(student);
                _unitOfWork.Commit();

                course.Students.Add(student);

                return student;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }
    }
}
