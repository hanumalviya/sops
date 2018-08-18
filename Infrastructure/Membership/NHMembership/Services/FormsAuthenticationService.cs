using System.Web.Security;
using FluentNHibernate.Cfg.Db;
using NHMembership.Logging;
using NHMembership.Security.Encryption;
using System;
using NHibernate.Cfg;

namespace NHMembership.Services
{
    public class FormsAuthenticationService : AuthenticationServiceBase, IAuthenticationService
    {
        public FormsAuthenticationService(string applicationName, IEncryptionStrategy encryptionStrategy, ILogger logger, IPersistenceConfigurer persistenceConfigurer, Action<Configuration> exposeConfiguration)
            : base(applicationName, encryptionStrategy, logger, persistenceConfigurer, exposeConfiguration)
        {
        }

        public override bool Login(string userName, string password, bool createPersistentCookie = false)
        {
            if (System.Web.Security.Membership.ValidateUser(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
                return true;
            }

            return false;
        }

        public override void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}
