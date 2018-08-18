using Model.Students;
using Model.University;
using System;
using System.Linq;

namespace SOPS.Services.Students
{
    public interface IStudentCreator
    {
        Student Create(
            string firstName,
            string lastName,
            string album,
            Course course,
            Mode mode,
            string email = "",
            string phone = "",
            string city = "",
            string address = "",
            string postalCode = "");
    }
}
