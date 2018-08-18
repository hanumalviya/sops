using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Security;

namespace Sops.Services.IntegrationTests.Employees
{
    [TestFixture]
    public class AuthenticationTest : TestBase
    {

        [SetUp]
        public void Setup()
        {
            //remove users
            var allUsers = authenticationService.MembershipService.AllUsers();
            foreach (var user in allUsers)
            {
                authenticationService.MembershipService.DeleteUser(user.UserName);
            }

            var users = authenticationService.MembershipService.AllUsers();
            Assert.That(users, Is.Empty, "users collection should be empty");


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
        public void CanRegisterUser()
        {
            var status1 = authenticationService.Register("user1", "qwerty", "test1@email.com", "question", "answer", true);
            var status2 = authenticationService.Register("user2", "qwerty", "test2@email.com", "question", "answer", true);

            var users = authenticationService.MembershipService.AllUsers();

            Assert.That(status1, Is.EqualTo(MembershipCreateStatus.Success));
            Assert.That(status2, Is.EqualTo(MembershipCreateStatus.Success)); 
            Assert.That(users, Is.Not.Empty, "users collection shouldn't be empty");
            Assert.That(users.Count, Is.EqualTo(2), "users collection should contain two users");
        }

        [Test]
        public void CanRemoveUser()
        {
            var status1 = authenticationService.Register("user1", "qwerty", "test1@email.com", "question", "answer", true);
            var status2 = authenticationService.Register("user2", "qwerty", "test2@email.com", "question", "answer", true);

            var users = authenticationService.MembershipService.AllUsers();
            Assert.That(users.Count, Is.EqualTo(2), "users collection should contain two users");

            authenticationService.MembershipService.DeleteUser("user1");
            users = authenticationService.MembershipService.AllUsers();
            Assert.That(users.Count, Is.EqualTo(1), "users collection should contain two users");
            Assert.That(users.First().Membership.Email, Is.EqualTo("test2@email.com"), "User membership contains wrong data");
        }

        [Test]
        public void CanCreateRoles()
        {
            authenticationService.RoleService.AddRole("testRole");

            var roles = authenticationService.RoleService.AllRoles();

            Assert.That(roles, Is.Not.Empty, "Roles collection should not be empty");
            Assert.Contains("testRole", roles.Select(n => n.Name).ToList(), "roles colection should contains testRole");
        }

        [Test]
        public void CanAddUserToRole()
        {
            authenticationService.RoleService.AddRole("testRole");
            authenticationService.Register("user1", "qwerty", "test1@email.com", "question", "answer", true);

            authenticationService.RoleService.AddUserToRole("user1", "testRole");
            var isInRole = authenticationService.RoleService.IsUserInRole("user1", "testRole");
            Assert.IsTrue(isInRole, "user should be in role");

            authenticationService.RoleService.RemoveUserFromRole("user1", "testRole");
            isInRole = authenticationService.RoleService.IsUserInRole("user1", "testRole");
            Assert.IsFalse(isInRole, "user shouldn't be in role");
        }
    }
}
