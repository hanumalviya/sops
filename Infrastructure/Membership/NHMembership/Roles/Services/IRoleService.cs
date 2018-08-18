using NHMembership.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHMembership.Roles.Services
{
    public interface IRoleService
    {
        Role GetRole(string roleName);
        void AddRole(string roleName);
        void RemoveRole(string roleName);
        IList<Role> AllRoles();
        IList<string> GetRolesForUser(string userName);
        IList<UserProfile> GetUsersInRole(string role);
        bool IsUserInRole(string userName, string roleName);

        void AddUserToRole(string userName, string role);
        void AddUsersToRoles(string[] userNames, string[] roleNames);
        void RemoveUserFromRole(string userName, string role);
        void RemoveUsersFromRoles(IEnumerable<string> userNames, IEnumerable<string> roleNames);
        bool RoleExist(string roleName);
        IList<string> FindUsersInRole(string roleName, string userNameToMatch);   
    }
}
