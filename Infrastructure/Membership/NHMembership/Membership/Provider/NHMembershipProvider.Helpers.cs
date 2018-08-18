using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Security;
using NHMembership.Models;

namespace NHMembership.Membership.Provider
{
    public sealed partial class NHMembershipProvider : MembershipProvider
    {
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        private void ParseConfiguration(NameValueCollection config)
        {
            _applicationName = GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            _minRequiredNonAlphanumericCharacters =
                Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            _passwordStrengthRegularExpression =
                Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            _requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            _requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            WriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));
        }

        private MembershipUser GetMembershipUserFromUser(UserProfile user)
        {
            try
            {
                var u = new MembershipUser(Name,
                                   user.UserName,
                                   user.Id,
                                   user.Membership.Email,
                                   user.Membership.PasswordQuestion,
                                   string.Empty,
                                   user.Membership.IsApproved,
                                   user.Membership.IsLockedOut,
                                   user.Membership.CreationDate,
                                   user.Membership.LastLoginDate ??DateTime.MinValue,
                                   user.Membership.LastActivityDate ?? DateTime.MinValue,
                                   user.Membership.LastPasswordChangedDate ?? DateTime.MinValue,
                                   user.Membership.LastLockedOutDate ?? DateTime.MinValue);

                return u;
            }
            catch
            {
                return null;
            }
        }

        private void UpdatePasswordFailureCount(string userName)
        {
            try
            {
                UserProfile user = _membershipService.GetUserProfileByName(userName);

                int failureCount = user.Membership.FailedPasswordAttemptCount;
                DateTime windowStart = user.Membership.FailedPasswordAttemptWindowStart ?? DateTime.MinValue;
                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    _membershipService.UpdatePasswordFailureCount(userName, 1, DateTime.Now);
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        user.Membership.IsLockedOut = true;
                        user.Membership.LastLockedOutDate = DateTime.Now;
                        
                        _membershipService.LockOutUser(userName, DateTime.Now);
                    }
                    else
                    {
                        user.Membership.FailedPasswordAttemptCount = failureCount;
                    }
                }
            }
            catch (Exception e)
            {
                if (!WriteExceptionsToEventLog)
                   WriteToEventLog(e, "UpdateFailureCount");
                else
                    throw new ProviderException("Unable to update failure count and window start." + ExceptionMessage);
            }
        }

        private void UpdateAnswerFailureCount(string userName)
        {
            try
            {
                UserProfile user = _membershipService.GetUserProfileByName(userName);

                int failureCount = user.Membership.FailedPasswordAnswerAttemptCount;
                DateTime windowStart = user.Membership.FailedPasswordAnswerAttemptWindowStart ?? DateTime.MinValue;
                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    _membershipService.UpdatePasswordAnswerFailureCount(userName, 1, DateTime.Now);
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        user.Membership.IsLockedOut = true;
                        user.Membership.LastLockedOutDate = DateTime.Now;

                        _membershipService.LockOutUser(userName, DateTime.Now);
                    }
                    else
                    {
                        user.Membership.FailedPasswordAnswerAttemptCount = failureCount;
                    }
                }
            }
            catch (Exception e)
            {
                if (!WriteExceptionsToEventLog)
                    WriteToEventLog(e, "UpdateFailureCount");
                else
                    throw new ProviderException("Unable to update failure count and window start." + ExceptionMessage);
            }
        }

        private bool Verify(string value, string encryptedValue, string salt)
        {
            return _encryptionStrategy.Verify(value, encryptedValue, salt);
        }

        private void WriteToEventLog(Exception e, string action)
        {
            _logger.Log(action, e);
        }
    }
}