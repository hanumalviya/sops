using NHibernateRepository.UnitOfWork;
using NHMembership.Services;
using SOPS.Repositories.Factory;
using System;
using System.Linq;

namespace SOPS.Services.Employees
{
    public class EmployeeDestructor : IEmployeeDestructor
    {
        private readonly IRepositoriesFactory _repositoriesFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public EmployeeDestructor(IUnitOfWork unitOfWork, IRepositoriesFactory repositoriesFactory, IAuthenticationService authenticationService)
        {
            _repositoriesFactory = repositoriesFactory;
            _unitOfWork = unitOfWork;
            this._authenticationService = authenticationService;
        }

        public void Destroy(int id)
        {
            try
            {
                var employeesRepository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
                var employee = employeesRepository.FindBy(id);

                _authenticationService.MembershipService.DeleteUser(employee.UserName);
            }
            catch
            {
                try
                {
                    _unitOfWork.BeginTransaction();
                    var employeesRepository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
                    var employee = employeesRepository.FindBy(id);
                    employee = employeesRepository.All().FirstOrDefault(n => n.Id == id);
                    if (employee != null)
                    {
                        employeesRepository.Delete(employee);
                    }
                    _unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    _unitOfWork.Rollback();
                    throw e;
                }
            }
        }


        public bool CanBeDestroyed(int id, int currentEmployee)
        {
            var employeesRepository = _repositoriesFactory.CreateEmployeeRepository(_unitOfWork);
            var employee = employeesRepository.FindBy(id);
            var current = employeesRepository.FindBy(currentEmployee);

            //^R*	A'*	(^A+^A'+R') 
            //b != 1 && c == 1 && (a != 1 || c != 1 || d == 1)
            return employee.Keeper == false && !employee.Root && current.Administrator && (!employee.Administrator || !current.Administrator || current.Root);
        }
    }
}
