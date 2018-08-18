using Model.Employees;
using Model.University;
using NHibernateRepository.UnitOfWork;
using NUnit.Framework;
using SOPS.Services.Courses;
using SOPS.Services.Departments;
using SOPS.Services.Employees;
using SOPS.Services.Templates;
using System;
using System.Linq;

namespace Sops.Services.IntegrationTests.Courses
{
    [TestFixture]
    public class CourseTest: TestBase
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

            var departments = departmentProvider.GetAllDepartments();

            foreach (var item in departments)
            {
                departmentDestructor.Destroy(item.Id);
            }
        }

        [Test]
        public void CanCreateDepartmentAndCourse()
        {
            var department = departmentCreator.Create("Wydział");

            var course = courseCreator.Create("kurs1", department);

            var currentDepartment = departmentProvider.GetDepartment("Wydział");

            Assert.That(department, Is.Not.Null, "Current department shouldnt be null");
            Assert.That(currentDepartment.Courses, Is.Not.Empty, "Courses should contain one item");
            Assert.That(course.Department.Name, Is.EqualTo("Wydział"), "course deparment is not equal to expected");
        }

        [Test]
        public void CanSetManagerTest()
        {
            //arranga

            var department = departmentCreator.Create("Wydział");
            var course = courseCreator.Create("kurs", department);           

            EmployeeCreateStatus status;
            var emp = employeeCreator.Create("testUser", "firstName", "lastName", "qwerty", "email@email.com", "q", "answer", true, true, true, course, out status);

            // act
            var currentCourse = courseProvider.GetCourse(course.Id);
            courseUpdater.SetManager(currentCourse, emp);
            courseUpdater.Update(course);

            // assert
            var checkCourse = courseProvider.GetCourse(currentCourse.Id);

            Assert.That(checkCourse.Manager, Is.Not.Null, "Course manager shouldn't be null");
            Assert.That(checkCourse.Manager.UserName, Is.EqualTo("testUser"), "Course manager is not equal to expected");
            Assert.IsTrue(checkCourse.Manager.Keeper, "Keeper property should be true");
            Assert.IsTrue(authenticationService.RoleService.IsUserInRole(checkCourse.Manager.UserName, EmployeesRoles.Keeper), "Employee should be in role keeper");

            // act
            currentCourse = courseProvider.GetCourse(currentCourse.Id);
            courseUpdater.SetManager(currentCourse, null);
            courseUpdater.Update(currentCourse);

            // assert
            checkCourse = courseProvider.GetCourse(currentCourse.Id);
            emp = employeesProvider.GetEmployee("testUser");

            Assert.That(checkCourse.Manager, Is.Null, "Course manager should be null");
            Assert.IsFalse(emp.Keeper, "Keeper property should be false");
            Assert.IsFalse(authenticationService.RoleService.IsUserInRole(emp.UserName, EmployeesRoles.Keeper), "Employee shouldn't be in role keeper");
        }

        [Test]
        public void CanDeleteCourse()
        {
            var department = departmentCreator.Create("Wydział");               
            
            var course1 = courseCreator.Create("kurs1", department.Id);
            var course2 = courseCreator.Create("kurs2", department.Id);

            var currentDepartment = departmentProvider.GetDepartment("Wydział");
            var currentCourse = courseProvider.GetCourse("kurs1");
            courseDestructor.Destroy(currentCourse.Id);

            Assert.That(currentDepartment, Is.Not.Null, "Current department shouldnt be null");
            Assert.That(currentDepartment.Courses, Is.Not.Empty, "Courses should contain one item");
            Assert.That(currentDepartment.Courses.Count, Is.EqualTo(1));
        }

        [Test]
        public void CanSetTemplate()
        {
            var newDepartment = departmentCreator.Create("department");
            var course = courseCreator.Create("kurs1", newDepartment);

            string DocXContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            var template = templateCreator.Create("testTemplate", "D:/templates/t.docx", DocXContentType);

            course.SetTemplate(template);
            courseUpdater.Update(course);

            var actualCourse = courseProvider.GetCourse("kurs1");
            var actualTemplate = templateProvider.GetTemplate(template.Id);

            Assert.That(actualCourse.Template.Name, Is.EqualTo("testTemplate"), "Template name is not equal to expected");
            Assert.IsTrue(actualTemplate.Courses.Any(n => n.Name == "kurs1" ) , "Actual template should contain kurs1");

            Assert.IsFalse(templateDestructor.CanBeDestroyed(actualTemplate.Id), "template can't be destroyed");
            actualCourse.SetTemplate(null);
            courseUpdater.Update(actualCourse);

            actualTemplate = templateProvider.GetTemplate(template.Id);
            Assert.That(actualCourse.Template, Is.Null, "Template should be null");
        }
    }
}
