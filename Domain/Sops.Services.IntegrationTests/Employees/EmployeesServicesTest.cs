using Model.Employees;
using NHibernateRepository.UnitOfWork;
using NUnit.Framework;
using SOPS.Services.Employees;
using System;
using System.Linq;

namespace Sops.Services.IntegrationTests.Employees
{
    [TestFixture] 
    public class EmployeesServicesTest : TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            RemoveTrashes();

            authenticationService.RoleService.AddRole(EmployeesRoles.Administrator);
            authenticationService.RoleService.AddRole(EmployeesRoles.Keeper);
            authenticationService.RoleService.AddRole(EmployeesRoles.Moderator);
            authenticationService.RoleService.AddRole(EmployeesRoles.Root);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
        
        private void RemoveTrashes()
        {
            //remove users
            var employees = employeesProvider.GetEmployees();

            foreach (var e in employees)
            {
                employeesDestructor.Destroy(e.Id);
            }

            //remove roles
            var allRoles = authenticationService.RoleService.AllRoles();
            foreach (var role in allRoles)
            {
                authenticationService.RoleService.RemoveRole(role.Name);
            }
            var roles = authenticationService.RoleService.AllRoles();
            Assert.That(roles, Is.Empty, "roles collection should be empty");
        }

        [Test]
        public void CanCreateEmployee()
        {                
            var department = departmentCreator.Create("testDepartmentasd");
            var course1 = courseCreator.Create("testCourse2", department);

            EmployeeCreateStatus result;
            var employee  = employeeCreator.Create("user1", "firstName", "lastName", "password", "test@enauk.com", "question", "answer", true, true, true, course1, out result);

            Assert.That(result, Is.EqualTo(EmployeeCreateStatus.Success), "status should be equal to success");
            Assert.That(employee, Is.Not.Null, "employee shouldn't be null");
            Assert.That(employee.FirstName = "firstName", Is.Not.Null, "employee's name is not equal to expected");
        }

        [Test]
        public void CanRemoveEmployee()
        {
            // arrange
            var department = departmentCreator.Create("testDepartmentasd");
            var course1 = courseCreator.Create("testCourse2", department);
            EmployeeCreateStatus result;
            var employee = employeeCreator.Create("user1", "firstName", "lastName", "password", "test@enauk.com", "question", "answer", true, true, true, course1, out result);

            // act
            employeesDestructor.Destroy(employee.Id);

            //assert
            var currentEmployee = employeesProvider.GetEmployee("user1");
            Assert.That(currentEmployee, Is.Null, "employee should be null");
        }

        [Test]
        public void CanUpdateEmployee()
        {
            using (var unitOfWork = new UnitOfWork(sessionFactory))
            {
                var department = departmentCreator.Create("testDepartmentasd");
                var course1 = courseCreator.Create("testCourse2", department);

                EmployeeCreateStatus result;
                var employee = employeeCreator.Create("user1", "firstName", "lastName", "password", "test@enauk.com", "question", "answer", true, true, true, course1, out result);

                employee.Keeper = true;
                employee.Administrator = false;
                employee.Root = false;
                employee.Moderator = false;

                employeesUpdater.Update(employee);
                var currentEmployee = employeesProvider.GetEmployee("user1");
                Assert.IsFalse(currentEmployee.Root, "root property should be false");
                Assert.IsFalse(authenticationService.RoleService.IsUserInRole("user1", EmployeesRoles.Root), "user shouldn't be in role root");

                Assert.IsFalse(currentEmployee.Administrator, "Administrator property should be false");
                Assert.IsFalse(authenticationService.RoleService.IsUserInRole("user1", EmployeesRoles.Administrator), "user shouldn't be in role Administrator");

                Assert.IsFalse(currentEmployee.Moderator, "Moderator property should be false");
                Assert.IsFalse(authenticationService.RoleService.IsUserInRole("user1", EmployeesRoles.Moderator), "user shouldn't be in role Moderator");
            }
        }
    }
}
