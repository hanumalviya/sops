using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Security;

namespace NHMembership.Roles.Provider
{
    public sealed partial class NHRoleProvider : RoleProvider
    {
        private void ParseConfig(NameValueCollection config)
        {
            _applicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            WriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        private void WriteToEventLog(Exception e, string action)
        {
            _logger.Log(action, e);
        }
    }
}