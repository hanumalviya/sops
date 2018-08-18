using System;
using System.Linq;
using Model.Employees;
using Model.University;

namespace SOPS.Services.Employees
{
    public interface IEmployeeCreator
    {
        Employee Create(string userName,
                    string firstName,
                    string lastName,
                    string password,
                    string email,
                    string question,
                    string answer,
                    bool moderator,
                    bool root,
                    bool administrator,
                    Course course,
                    out EmployeeCreateStatus status);
    }

    public enum EmployeeCreateStatus
    {
        Success,
        DuplicateMail,
        UserAlreadyExist,
        None,
        InvalidAnswer,
        UserRejected,
        ProviderError,
        InvalidUserName,
        InvalidQuestion,
        InvalidPassword,
        InvalidEmail,

    }
}
