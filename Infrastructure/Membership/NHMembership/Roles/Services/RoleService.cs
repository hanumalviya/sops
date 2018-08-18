using NHMembership.DataAccess.Roles;
using NHMembership.DataAccess.Users;
using NHMembership.Exceptions;
using NHMembership.Models;
using NHibernateRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHMembership.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly string _applicationName;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUsersRepositoryFactory _usersRepositoryFactory;
        private readonly IRolesRepositoryFactory _rolesRepositoryFactory;

        public RoleService(string applicationName, IUnitOfWorkFactory unitOfWorkFactory, IUsersRepositoryFactory usersRepositoryFactory, IRolesRepositoryFactory rolesRepositoryFactory)
        {
            _applicationName = applicationName;
            _unitOfWorkFactory = unitOfWorkFactory;;
            _usersRepositoryFactory = usersRepositoryFactory;
            _rolesRepositoryFactory = rolesRepositoryFactory;
        }

        public Models.Role GetRole(string roleName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                try
                {
                    return rolesRepository.FindBy(n => n.Name == roleName && n.ApplicationName == _applicationName);
                }
                catch (Exception e)
                {
                    throw new RoleServiceException(string.Format("An error occurred when invoked GetRole({0}) method.", roleName), e);
                }
            }
        }

        public void AddRole(string roleName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                try
                {
                    unitOfWork.BeginTransaction();

                    var role = new Role
                    {
                        ApplicationName = _applicationName,
                        Name = roleName
                    };
                    rolesRepository.Add(role);

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();

                    throw new RoleServiceException(string.Format("An error occurred when invoked AddRole({0}) method.", roleName), e);
                }
            }
        }

        public void RemoveRole(string roleName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);
                    var role = rolesRepository.FindBy(n => n.Name == roleName && n.ApplicationName == _applicationName);

                    rolesRepository.Delete(role);

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();

                    throw new RoleServiceException(string.Format("An error occurred when invoked RemoveRole({0}) method.", roleName), e);
                }
            }
        }

        public IList<Models.Role> AllRoles()
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                    return rolesRepository.FilterBy(n => n.ApplicationName == _applicationName).ToList();
                }
                catch (Exception e)
                {
                    throw new RoleServiceException("An error occurred when invoked All() method.", e);
                }
            }
        }

        public IList<string> GetRolesForUser(string userName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork); 

                try
                {
                    unitOfWork.BeginTransaction();

                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);

                    unitOfWork.Commit();
                    return user.Roles.Select(n => n.Name).ToList();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new RoleServiceException(
                        string.Format("An error occurred when invoked GetRolesForUser({0}) method.", userName), e);
                }

            }
        }

        public IList<Models.UserProfile> GetUsersInRole(string roleName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);
                try
                {
                    var role = rolesRepository.FindBy(n => n.Name == roleName && n.ApplicationName == _applicationName);

                    return role.UsersInRole;
                }
                catch (Exception e)
                {
                    throw new RoleServiceException(
                        string.Format("An error occurred when invoked GetUsersInRole({0}) method.", roleName), e);
                }
            }
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);
                try
                {
                    var role = rolesRepository.FindBy(n => n.Name == roleName && n.ApplicationName == _applicationName);

                    return role.UsersInRole.Any(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                }
                catch (Exception e)
                {
                    throw new RoleServiceException(
                        string.Format("An error occurred when invoked IsUserInRole({0}) method.", roleName), e);
                }
            }
        }

        public void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                    foreach (var userName in userNames)
                    {
                        var user = usersRepository.FindBy(n => n.UserName == userName && 
                            n.Membership.ApplicationName == _applicationName);

                        foreach (var roleName in roleNames)
                        {
                            var role = rolesRepository.FindBy(n => n.Name == roleName && n.ApplicationName == _applicationName);
                            user.AddRole(role);
                        }
                    }

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new RoleServiceException("An error occurred when invoked RemoveUsersFromRoles(...) method.", e);
                }
            }
        }

        public void RemoveUsersFromRoles(IEnumerable<string> userNames, IEnumerable<string> roleNames)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                    foreach (var userName in userNames)
                    {
                        var user = usersRepository.FindBy(n => n.UserName == userName && 
                            n.Membership.ApplicationName == _applicationName);

                        foreach (var r in roleNames)
                        {
                            var role = rolesRepository.FindBy(n => n.Name == r && n.ApplicationName == _applicationName);
                            user.RemoveRole(role);
                        }
                    }

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new RoleServiceException("An error occurred when invoked RemoveUsersFromRoles(...) method.", e);
                }
            }
        }


        public bool RoleExist(string roleName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                    return rolesRepository.All().Any(n => n.ApplicationName == _applicationName && n.Name == roleName);
                }
                catch (Exception e)
                {
                    throw new RoleServiceException(
                        string.Format("An error occurred when invoked RoleExist({0}) method.", roleName), e);
                }
            }
        }

        public IList<string> FindUsersInRole(string roleName, string userNameToMatch)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);

                try
                {
                    var userNames = from user in usersRepository.All()
                                    where user.UserName == userNameToMatch &&
                                          user.Membership.ApplicationName == _applicationName &&
                                          user.Roles.Any(n => n.Name == roleName)
                                    select user.UserName;

                    return userNames.ToList();
                }
                catch (Exception e)
                {
                    throw new RoleServiceException(
                        string.Format("An error occurred when invoked FindUsersInRole({0}) method.", roleName), e);
                }
            }
        }


        public void AddUserToRole(string userName, string role)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                    var r = rolesRepository.FindBy(n => n.Name == role && n.ApplicationName == this._applicationName);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == this._applicationName);
                    user.AddRole(r);

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new RoleServiceException("An error occurred when invoked RemoveUserFromRole(...) method.", e);
                }
            }
        }

        public void RemoveUserFromRole(string userName, string role)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var rolesRepository = _rolesRepositoryFactory.CreateRolesRepository(unitOfWork);

                    var r = rolesRepository.FindBy(n => n.Name == role && n.ApplicationName == this._applicationName);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == this._applicationName);
                    user.RemoveRole(r);

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new RoleServiceException("An error occurred when invoked RemoveUserFromRole(...) method.", e);
                }
            }
        }
    }
}
