using Model.Employees;
using Model.University;
using NHibernateRepository.UnitOfWork;
using NHMembership.Services;
using SOPS.Repositories.Factory;
using System;
using System.Linq;
using System.Web.Security;

namespace SOPS.Services.Employees
{
    public class EmployeeCreator : IEmployeeCreator
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public EmployeeCreator(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IRepositoriesFactory repositoriesFactory)
        {
            _repositoriesFactory = repositoriesFactory;
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
        }

        public Employee Create(string userName,
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
            out EmployeeCreateStatus status)
        {
            status = EmployeeCreateStatus.None;
            try
            {
                var s = _authenticationService.Register(userName, password, email, question, answer, true);
                status = ConvertStatus(s);

                if (status == EmployeeCreateStatus.Success)
                {
                    _unitOfWork.BeginTransaction();

                    if (administrator)
                        _authenticationService.RoleService.AddUserToRole(userName, EmployeesRoles.Administrator);
                    if (moderator)
                        _authenticationService.RoleService.AddUserToRole(userName, EmployeesRoles.Moderator);
                    if (root)
                        _authenticationService.RoleService.AddUserToRole(userName, EmployeesRoles.Root);

                    var employeesRepository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
                    var userId = _authenticationService.MembershipService.GetUserProfileByName(userName).Id;
                    
                    var employee = employeesRepository.FindBy(userId);
                    employee.FirstName = firstName;
                    employee.LastName = lastName;
                    employee.Email = email;
                    employee.Moderator = moderator;
                    employee.Root = root;
                    employee.Administrator = administrator;
                    employee.Course = course;

                    _unitOfWork.Commit();

                    return employee;
                }

                return null;
            }
            catch(Exception e)
            {
                if (status == EmployeeCreateStatus.Success)
                {
                    _unitOfWork.Rollback();

                    if (_authenticationService.RoleService.IsUserInRole(userName, EmployeesRoles.Administrator))
                        _authenticationService.RoleService.RemoveUserFromRole(userName, EmployeesRoles.Administrator);
                    if (_authenticationService.RoleService.IsUserInRole(userName, EmployeesRoles.Root))
                        _authenticationService.RoleService.RemoveUserFromRole(userName, EmployeesRoles.Root);
                    if (_authenticationService.RoleService.IsUserInRole(userName, EmployeesRoles.Moderator))
                        _authenticationService.RoleService.RemoveUserFromRole(userName, EmployeesRoles.Moderator);

                    _authenticationService.MembershipService.DeleteUser(userName);
                }

                throw e;
            }
        }

        private EmployeeCreateStatus ConvertStatus(MembershipCreateStatus status)
        {
            EmployeeCreateStatus result;

            switch (status)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    result = EmployeeCreateStatus.DuplicateMail;
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                case MembershipCreateStatus.DuplicateUserName:
                    result = EmployeeCreateStatus.UserAlreadyExist;
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    result = EmployeeCreateStatus.InvalidAnswer;
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    result = EmployeeCreateStatus.InvalidEmail;
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    result = EmployeeCreateStatus.InvalidPassword;
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    result = EmployeeCreateStatus.InvalidQuestion;
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    result = EmployeeCreateStatus.InvalidUserName;
                    break;
                case MembershipCreateStatus.ProviderError:
                    result = EmployeeCreateStatus.ProviderError;
                    break;
                case MembershipCreateStatus.Success:
                    result = EmployeeCreateStatus.Success;
                    break;
                case MembershipCreateStatus.UserRejected:
                    result = EmployeeCreateStatus.UserRejected;
                    break;
                default:
                    result = EmployeeCreateStatus.None;
                    break;
            }

            return result;
        }
    }


}
