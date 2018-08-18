using NHMembership.Membership.Services;
using NHMembership.Roles.Services;
using System;
using System.Linq;
using System.Web.Security;

namespace NHMembership.Services
{
    public interface IAuthenticationService
    {
        bool Login(string userName, string password, bool createPersistentCookie = false);
        void Logout();

        MembershipCreateStatus Register(string userName, string password, string email, string question, string answer, bool approved);
        void Configure();

        IMembershipService MembershipService { get; }
        IRoleService RoleService { get; }

        bool ChangePassword(int userId, string oldPassword, string newPassword, string question, string answer);
        string ResetPassword(int userId, string answer);
    }
}
