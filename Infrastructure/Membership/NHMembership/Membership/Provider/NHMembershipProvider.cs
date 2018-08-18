using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;
using NHMembership.Logging;
using NHMembership.Models;
using NHMembership.Membership.Services;
using NHMembership.Exceptions;
using NHMembership.Security.Encryption;

namespace NHMembership.Membership.Provider
{
    public sealed partial class NHMembershipProvider : MembershipProvider
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name.Length == 0)
                name = "NHMemebershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Nhibernate Membership provider");
            }

            base.Initialize(name, config);
            ParseConfiguration(config);
        }

        public void Configure(IMembershipService service, IEncryptionStrategy encryptionStrategy, ILogger logger)
        {
            _logger = logger;
            _encryptionStrategy = encryptionStrategy;
            _membershipService = service;
        }

        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            try
            {
                if (!ValidateUser(username, oldPwd))
                    return false;

                var args = new ValidatePasswordEventArgs(username, newPwd, true);

                OnValidatingPassword(args);

                if (args.Cancel)
                    if (args.FailureInformation != null)
                        throw args.FailureInformation;
                    else
                        throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

                string newSalt;
                _membershipService.ChangeUserPassword(username, _encryptionStrategy.Encrypt(newPwd, out newSalt), newSalt, DateTime.Now);

                return true;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");
                    return false;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }


        public override bool ChangePasswordQuestionAndAnswer(string userName,
                                                             string password,
                                                             string newQuestion,
                                                             string newAnswer)
        {
            try
            {
                if (ValidateUser(userName, password) == false)
                    return false;

                string newSalt;
                _membershipService.ChangeUserQuestionAndAnswer(userName, newQuestion,
                                                               _encryptionStrategy.Encrypt(newAnswer, out newSalt),
                                                               newSalt);
                return true;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");
                    return false;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override MembershipUser CreateUser(string userName,
                                                  string password,
                                                  string email,
                                                  string passwordQuestion,
                                                  string passwordAnswer,
                                                  bool isApproved,
                                                  object providerUserKey,
                                                  out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(userName, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            if (GetUser(userName, false) != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            try
            {
                string passwordSalt;
                string passwordAnswerSalt;
                _membershipService.CreateUser(userName,
                                                _encryptionStrategy.Encrypt(password, out passwordSalt),
                                                passwordSalt,
                                                email,
                                                passwordQuestion,
                                                _encryptionStrategy.Encrypt(passwordAnswer, out passwordAnswerSalt),
                                                passwordAnswerSalt,
                                                isApproved,
                                                DateTime.Now);
                status = MembershipCreateStatus.Success;

                return GetUser(userName, false);
            }
            catch (Exception e)
            {
                status = MembershipCreateStatus.ProviderError;
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateUser");
                    return null;
                }
                throw;
            }
        }

        public override bool DeleteUser(string userName, bool deleteAllRelatedData)
        {
            try
            {
                _membershipService.DeleteUser(userName);
                return true;
            }
            catch (MembershipServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteUser");
                    return false;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var users = new MembershipUserCollection();
            totalRecords = 0;
            try
            {
                var all = _membershipService.AllUsers();
                totalRecords = all.Count();
                var selectedUsers = all.Skip(pageIndex * pageSize).TakeWhile((p, i) => i < pageSize);

                foreach (var u in selectedUsers)
                {
                    MembershipUser mu = GetMembershipUserFromUser(u);
                    users.Add(mu);
                }

                return users;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers");
                    return null;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            var onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            var compareTime = DateTime.Now.Subtract(onlineSpan);
            int numOnline = 0;

            try
            {
                numOnline = _membershipService.GetNumberOfUsersOnline(compareTime);
            }
            catch (MembershipServiceException e)
            {
                if (WriteExceptionsToEventLog)
                    WriteToEventLog(e, "GetNumberOfUsersOnline");
                else
                    throw new ProviderException(ExceptionMessage);
            }

            return numOnline;
        }

        public override string GetPassword(string username, string answer)
        {
            return String.Empty;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            try
            {
                if (userIsOnline)
                    _membershipService.UpdateLastUserActivityDate(username, DateTime.Now);

                var user = _membershipService.GetUserProfileByName(username);
                return this.GetMembershipUserFromUser(user);
            }
            catch (MembershipServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");
                    return null;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            try
            {
                var user = _membershipService.GetUserProfileByKey((int)providerUserKey);

                if (userIsOnline)
                    _membershipService.UpdateLastUserActivityDate(user.UserName, DateTime.Now);

                return this.GetMembershipUserFromUser(user);
            }
            catch (MembershipServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");
                    return null;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override bool UnlockUser(string userName)
        {
            try
            {
                _membershipService.UnLockUser(userName, DateTime.Now);

                return true;
            }
            catch (MembershipServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");
                    return false;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            try
            {
                return _membershipService.GetUserNameByEmail(email);
            }
            catch (MembershipServiceException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");
                    return string.Empty;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override string ResetPassword(string userName, string answer)
        {
            if (!EnablePasswordReset)
                throw new NotSupportedException("Password reset is not enabled.");

            if (answer == string.Empty)
            {
                UpdateAnswerFailureCount(userName);
                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword = System.Web.Security.Membership.GeneratePassword(NewPasswordLength,
                                                               MinRequiredNonAlphanumericCharacters);

            var args = new ValidatePasswordEventArgs(userName, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                
                throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
            }

            try
            {
                UserProfile userProfile = _membershipService.GetUserProfileByName(userName);

                if (userProfile.Membership.IsLockedOut)
                    throw new MembershipPasswordException("The supplied user is locked out.");

                var passwordAnswer = userProfile.Membership.PasswordAnswer;
                var passwordAnswerSalt = userProfile.Membership.PasswordAnswerSalt;

                if (!Verify(answer, passwordAnswer, passwordAnswerSalt))
                {
                    UpdateAnswerFailureCount(userName);
                    throw new MembershipPasswordException("Incorrect password answer.");
                }

                string passwordSalt;
                _membershipService.ChangeUserPassword(userName, 
                    _encryptionStrategy.Encrypt(newPassword, out passwordSalt),
                    passwordSalt, 
                    DateTime.Now);

                return newPassword;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                    WriteToEventLog(e, "ResetPassword");

                throw new ProviderException(ExceptionMessage, e);
            }
        }

        public override void UpdateUser(MembershipUser user)
        {
            try
            {
                _membershipService.UpdateUser(user.UserName, user.Email, user.IsApproved);
            }
            catch (Exception  e)
            {
                if (WriteExceptionsToEventLog)
                    WriteToEventLog(e, "UpdateUser");
                else
                    throw new ProviderException(ExceptionMessage);
            }
        }

        public override bool ValidateUser(string userName, string password)
        {
            try
            {
                UserProfile userProfile = _membershipService.GetUserProfileByName(userName);
                if (userProfile == null)
                    return false;
                if (userProfile.Membership.IsLockedOut)
                    return false;

                string salt = userProfile.Membership.PasswordSalt;

                if (Verify(password, userProfile.Membership.Password, salt))
                {
                    if (userProfile.Membership.IsApproved)
                    {
                        _membershipService.UpdateLastUserLoginDate(userName, DateTime.Now);
                        return true;
                    }
                }
                else
                    UpdatePasswordFailureCount(userName);

                return false;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");
                    return false;
                }
                throw new ProviderException(ExceptionMessage, e);
            }
        }

        public override MembershipUserCollection FindUsersByName(string userName, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            var users = new MembershipUserCollection();
            totalRecords = 0;
            try
            {
                var all = _membershipService.GetUsersByName(userName);
                totalRecords = all.Count();
                var selectedUsers = all.Skip(pageIndex * pageSize).TakeWhile((p, i) => i < pageSize);
               
                foreach (var u in selectedUsers)
                {
                    MembershipUser mu = GetMembershipUserFromUser(u);
                    users.Add(mu);
                }

                return users;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByName");
                    return null;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            var users = new MembershipUserCollection();
            totalRecords = 0;
            try
            {
                var all = _membershipService.GetUsersByEmail(emailToMatch);
                totalRecords = all.Count();
                var selectedUsers = all.Skip(pageIndex * pageSize).TakeWhile((p, i) => i < pageSize);

                foreach (var u in selectedUsers)
                {
                    MembershipUser mu = GetMembershipUserFromUser(u);
                    users.Add(mu);
                }

                return users;
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByEmail");
                    return null;
                }
                throw new ProviderException(ExceptionMessage);
            }
        }
    }
}