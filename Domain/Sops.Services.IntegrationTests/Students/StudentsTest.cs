using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Sops.Services.IntegrationTests.Students
{
    [TestFixture]
    class StudentsTest : TestBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void CanCreateStudent()
        {
            var department = departmentCreator.Create("testDepartment");

            Assert.That(department.Id, Is.Not.EqualTo(0), "department id should be different than 0");
            var course = courseCreator.Create("testCourse", department.Id);
            var mode = modesCreator.Create("testMode");

            var student = studentsCreator.Create("firstName", "lastName", "album", course, mode);
            
            var currenStudent = studentsProvider.GetStudent(student.Id);
            Assert.That(currenStudent.Course.Students.Count, Is.EqualTo(1), "course should contain one item");
            
            studentDestructor.Destroy(student.Id);
            currenStudent = studentsProvider.GetStudent(student.Id);
            Assert.That(currenStudent, Is.Null, "course should be null"); 
        }

        [Test]
        public void StudentImportTest()
        {
            var department = departmentCreator.Create("testDepartment");
            var course = courseCreator.Create("testCourse", department.Id);
            var mode = modesCreator.Create("testMode");
            var s = File.OpenRead("Data/informatyka.txt");
            studentImporter.Import(s, course, mode);

            var students = studentsProvider.GetStudents(course.Id);
            Assert.That(students, Is.Not.Empty, "students list shouldn't be empty");
        }
    }
}
