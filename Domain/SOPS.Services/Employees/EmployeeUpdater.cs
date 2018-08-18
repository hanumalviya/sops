using Model.Employees;
using NHibernateRepository.UnitOfWork;
using NHMembership.Services;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Employees
{
    public class EmployeeUpdater : IEmployeeUpdater
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public EmployeeUpdater(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory, IAuthenticationService authenticationService)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        public void Update(Model.Employees.Employee employee)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var repository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
                var currentEmployee = repository.FindBy(employee.Id);

                RemoveFromRoles(employee, currentEmployee);
                AddToRoles(employee, currentEmployee);

                repository.Update(employee);
                _authenticationService.MembershipService.UpdateUser(employee.UserName, employee.Email, true);

                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw e;
            }
        }

        private void AddToRoles(Employee employee, Employee currentEmployee)
        {
            if (employee.Root == true &&
                _authenticationService.RoleService.IsUserInRole(currentEmployee.UserName, EmployeesRoles.Root) == false)
            {
                _authenticationService.RoleService.AddUserToRole(currentEmployee.UserName, EmployeesRoles.Root);
            }

            if (employee.Moderator == true &&
                _authenticationService.RoleService.IsUserInRole(currentEmployee.UserName, EmployeesRoles.Moderator) == false)
            {
                _authenticationService.RoleService.AddUserToRole(currentEmployee.UserName, EmployeesRoles.Moderator);
            }

            if (employee.Administrator == true &&
                _authenticationService.RoleService.IsUserInRole(currentEmployee.UserName, EmployeesRoles.Administrator) == false)
            {
                _authenticationService.RoleService.AddUserToRole(currentEmployee.UserName, EmployeesRoles.Administrator);
            }
        }
  
        private void RemoveFromRoles(Employee employee, Employee currentEmployee)
        {
            if (employee.Root == false &&
                _authenticationService.RoleService.IsUserInRole(currentEmployee.UserName, EmployeesRoles.Root))
            {
                _authenticationService.RoleService.RemoveUserFromRole(currentEmployee.UserName, EmployeesRoles.Root);
            }

            if (employee.Moderator == false &&
                _authenticationService.RoleService.IsUserInRole(currentEmployee.UserName, EmployeesRoles.Moderator))
            {
                _authenticationService.RoleService.RemoveUserFromRole(currentEmployee.UserName, EmployeesRoles.Moderator);
            }

            if (employee.Administrator == false &&
                _authenticationService.RoleService.IsUserInRole(currentEmployee.UserName, EmployeesRoles.Administrator))
            {
                _authenticationService.RoleService.RemoveUserFromRole(currentEmployee.UserName, EmployeesRoles.Administrator);
            }
        }
    }
}
