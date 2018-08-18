using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;
using NHMembership.Exceptions;
using NHMembership.Logging;
using NHMembership.Roles.Services;

namespace NHMembership.Roles.Provider
{
    public sealed partial class NHRoleProvider : RoleProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name.Length == 0)
                name = "FNHRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Fluent Nhibernate Role provider");
            }

            base.Initialize(name, config);

            ParseConfig(config);
        }

        public void Configure(IRoleService roleServices, ILogger logger)
        {
            _roleService = roleServices;
            _logger = logger;
        }

        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            try
            {
                _roleService.AddUsersToRoles(userNames, roleNames);
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                    WriteToEventLog(e, "AddUsersToRoles");
                else
                    throw;
            }
        }

        public override void CreateRole(string rolename)
        {
            try
            {
                if (rolename.Contains(","))
                    throw new ArgumentException("Role names cannot contain commas.");

                if (_roleService.RoleExist(rolename))
                    throw new ProviderException("Role name already exists.");

                _roleService.AddRole(rolename);
            }

            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                    WriteToEventLog(e, "CreateRole");
                else
                    throw;
            }
        }

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            try
            {
                if (throwOnPopulatedRole && _roleService.GetUsersInRole(rolename).Any())
                    throw new ProviderException("Cannot delete a populated role.");

                _roleService.RemoveRole(rolename);
                return true;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteRole");
                    return false;
                }
                throw;
            }
        }

        public override string[] GetAllRoles()
        {
            try
            {
                return _roleService.AllRoles().Select(n => n.Name).ToArray();
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllRoles");
                    return new string[] {};
                }
                throw;
            }
        }

        public override string[] GetRolesForUser(string userName)
        {
            try
            {
                return _roleService.GetRolesForUser(userName).ToArray();
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetRolesForUser");
                    return new string[] { };
                }
                throw;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            try
            {
                return _roleService.GetUsersInRole(roleName).Select(n => n.UserName).ToArray();
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUsersInRole");
                    return new string[]{};
                }
                throw;
            }
        }

        public override bool IsUserInRole(string username, string rolename)
        {
            try
            {
                return _roleService.IsUserInRole(username, rolename);
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "IsUserInRole");
                    return false;
                }
                throw;
            }
        }
        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            try
            {
                _roleService.RemoveUsersFromRoles(userNames, roleNames);
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                    WriteToEventLog(e, "RemoveUsersFromRoles");
                else
                    throw;
            }
        }

        public override bool RoleExists(string roleName)
        {
            try
            {
                return _roleService.RoleExist(roleName);
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RoleExists");
                    return false;
                }
                throw;
            }
        }

        public override string[] FindUsersInRole(string roleName, string userNameToMatch)
        {
            try
            {
                return _roleService.FindUsersInRole(roleName, userNameToMatch).ToArray();
            }
            catch (RoleServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersInRole");
                    return new string[]{};
                }
                throw;
            }
        }
    }
}