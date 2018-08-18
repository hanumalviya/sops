using System.Web.Security;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHMembership.DataAccess.Roles;
using NHMembership.DataAccess.Users;
using NHMembership.Logging;
using NHMembership.Membership.Provider;
using NHMembership.Models;
using NHMembership.Roles.Provider;
using System.Configuration.Provider;
using NHMembership.Membership.Services;
using NHMembership.Roles.Services;
using NHMembership.Security.Encryption;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernateRepository.UnitOfWork;
using System;
using NHibernate.Cfg;

namespace NHMembership.Services
{
    public abstract class AuthenticationServiceBase : IAuthenticationService
    {
        protected static bool _configured;
        protected static readonly object ConfigurationLocker = new object();

        protected readonly IRoleService _roleService;
        protected readonly IMembershipService _membershipService;
        protected readonly IEncryptionStrategy _encryptionStrategy;
        protected readonly ILogger _logger;
        protected NHMembershipProvider _membershipProvider;
        protected NHRoleProvider _roleProvider;

        public AuthenticationServiceBase(string applicationName, IEncryptionStrategy encryptionStrategy, ILogger logger, IPersistenceConfigurer persistenceConfigurer, Action<Configuration> exposeConfiguration)
        {
            ISessionFactory sessionFactory = CreateConfiguration(persistenceConfigurer, exposeConfiguration);
            IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory(sessionFactory);

            IUsersRepositoryFactory usersRepositoryFactory = new UsersRepositoryFactory();
            IRolesRepositoryFactory rolesRepositoryFactory = new RolesRepositoryFactory();

            _membershipService = new MembershipService(applicationName, unitOfWorkFactory, usersRepositoryFactory);
            _roleService = new RoleService(applicationName, unitOfWorkFactory, usersRepositoryFactory, rolesRepositoryFactory);
            _encryptionStrategy = encryptionStrategy;
            _logger = logger;
        }

        public abstract bool Login(string userName, string password, bool createPersistentCookie = false);
        public abstract void Logout();

        protected ISessionFactory CreateConfiguration(IPersistenceConfigurer persistenceConfigurer, Action<Configuration> exposeConfiguration)
        {
            ISessionFactory factory = Fluently.Configure()
                .Database(persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserProfile>())
                .ExposeConfiguration(exposeConfiguration)
                .BuildSessionFactory();

            return factory;
        }

        public void Configure()
        {
            lock (ConfigurationLocker)
            {
                try
                {
                    if (_configured == false)
                    {

                        _membershipProvider = System.Web.Security.Membership.Provider as NHMembershipProvider;
                        _roleProvider = System.Web.Security.Roles.Provider as NHRoleProvider;

                        _membershipProvider.Configure(_membershipService, _encryptionStrategy, _logger);
                        _roleProvider.Configure(_roleService, _logger);

                        _configured = true;
                    }
                }
                catch (System.Exception)
                {
                    _configured = false;
                    throw;
                } 
            }
        }

        public MembershipCreateStatus Register(string userName, string password, string email, string question, string answer, bool approved)
        {
            MembershipCreateStatus status;
            _membershipProvider.CreateUser(userName, password, email, question, answer, approved, null, out status);

            return status;
        }


        public IMembershipService MembershipService
        {
            get 
            {
                return _membershipService; 
            }
        }

        public IRoleService RoleService
        {
            get
            {
                return _roleService;
            }
        }


        public bool ChangePassword(int userId, string oldPassword, string newPassword, string question, string answer)
        {
            string userName = _membershipService.GetUserProfileByKey(userId).UserName;
            bool passwordChanged = _membershipProvider.ChangePassword(userName, oldPassword, newPassword);

            if (passwordChanged)
            {
               bool questionChanged =  _membershipProvider.ChangePasswordQuestionAndAnswer(userName, newPassword, question, answer);

               if (questionChanged == false)
               {
                   _membershipProvider.ChangePassword(userName, newPassword, oldPassword);
                   return false;
               }

               return true;
            }

            return false;
        }

        public string ResetPassword(int userId, string answer)
        {
            string userName = _membershipService.GetUserProfileByKey(userId).UserName;
            return _membershipProvider.ResetPassword(userName, answer);
        }
    }
}
