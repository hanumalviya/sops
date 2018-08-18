using System.Web.Security;
using NHMembership.Configurations.Database;
using NHMembership.Logging;
using NHMembership.Roles.Services;
using NHibernate;

namespace NHMembership.Roles.Provider
{
    public sealed partial class NHRoleProvider : RoleProvider
    {
        private string _applicationName;
        private ILogger _logger;
        private IRoleService _roleService;

        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public bool WriteExceptionsToEventLog { get; set; }
    }
}