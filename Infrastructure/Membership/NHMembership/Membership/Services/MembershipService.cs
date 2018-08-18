using System;
using System.Collections.Generic;
using System.Linq;
using NHMembership.DataAccess.Users;
using NHMembership.Exceptions;
using NHibernateRepository.UnitOfWork;
using NHMembership.Models;

namespace NHMembership.Membership.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly string _applicationName;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUsersRepositoryFactory _usersRepositoryFactory;

        public MembershipService(string applicationName, IUnitOfWorkFactory unitOfWorkFactory, IUsersRepositoryFactory usersRepositoryFactory)
        {
            _applicationName = applicationName;
            _unitOfWorkFactory = unitOfWorkFactory;;
            _usersRepositoryFactory = usersRepositoryFactory;
        }

        public IList<Models.UserProfile> GetUsersByEmail(string email)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);

                    return usersRepository.FilterBy(n => n.Membership.Email == email && n.Membership.ApplicationName == _applicationName).ToList();
                }
                catch (Exception e)
                {
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked GetUsersByEmail({0}) method.", email), e);
                }
            }
        }

        public IList<Models.UserProfile> GetUsersByName(string name)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);

                    return
                        usersRepository.FilterBy(
                            n => n.UserName == name && _applicationName == n.Membership.ApplicationName).ToList();
                }
                catch (Exception e)
                {
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked GetUsersByName({0}) method.", name), e);
                }
            }
        }

        public IList<Models.UserProfile> AllUsers()
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    return usersRepository.FilterBy(n => n.Membership.ApplicationName == _applicationName).ToList();
                }
                catch (Exception e)
                {
                    throw new MembershipServiceException("An error occurred when invoked AllUsers() method.", e);
                }
            }
        }

        public Models.UserProfile GetUserProfileByName(string userName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    return
                        usersRepository.FindBy(
                            n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                }
                catch (Exception e)
                {
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked GetUserProfileByName({0}) method.", userName), e);
                }
            }
        }

        public Models.UserProfile GetUserProfileByKey(int id)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    return
                        usersRepository.FindBy(
                            n => n.Id == id && n.Membership.ApplicationName == _applicationName);
                }
                catch (Exception e)
                {
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked GetUserProfileByKey({0}) method.", id), e);
                }
            }
        }

        public void UpdatePasswordFailureCount(string userName, int count, DateTime attemptTime)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);

                    user.Membership.FailedPasswordAttemptCount = count;
                    user.Membership.FailedPasswordAttemptWindowStart = attemptTime;
                    usersRepository.Update(user);

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked UpdatePasswordFailureCount({0}) method.", userName), e);
                }
            }
        }

        public void UpdatePasswordAnswerFailureCount(string userName, int count, DateTime attemptTime)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.FailedPasswordAnswerAttemptCount = count;
                    user.Membership.FailedPasswordAnswerAttemptWindowStart = attemptTime;
                    usersRepository.Update(user);

                    unitOfWork.Commit();

                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked UpdatePasswordAnswerFailureCount({0}) method.", userName), e);
                }
            }
        }

        public void ChangeUserPassword(string userName, string password, string passwordSalt, DateTime lastPasswordChangedDate)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.Password = password;
                    user.Membership.PasswordSalt = passwordSalt;
                    user.Membership.LastPasswordChangedDate = lastPasswordChangedDate;
                    
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked ChangeUserPassword({0}) method.", userName), e);
                }
            }
        }

        public void ChangeUserQuestionAndAnswer(string userName, string newQuestion, string answer, string answerSalt)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.PasswordQuestion = newQuestion;
                    user.Membership.PasswordAnswer = answer;
                    user.Membership.PasswordAnswerSalt = answerSalt;

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked ChangeUserQuestionAndAnswer({0}) method.", userName), e);
                }
            }
        }

        public void CreateUser(string username, string password, string passwordSalt, string email, string passwordQuestion, string passwordAnswer, string passwordAnswerSalt, bool isApproved, DateTime creationDate)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = new UserProfile();
                    user.UserName = username;
                    var userMembership = new UserMembership(user)
                        {
                            ApplicationName = _applicationName,
                            Password = password,
                            PasswordSalt = passwordSalt,
                            Email = email,
                            PasswordQuestion = passwordQuestion,
                            PasswordAnswer = passwordAnswer,
                            PasswordAnswerSalt = passwordAnswerSalt,
                            IsApproved = isApproved,
                            CreationDate = creationDate,
                            FailedPasswordAnswerAttemptCount = 0,
                            FailedPasswordAttemptCount = 0,
                            IsLockedOut = false
                        };

                    usersRepository.Add(user);
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked CreateUser({0}) method.", username), e);
                }
            }
        }

        public void DeleteUser(string userName)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);

                    usersRepository.Delete(user);
                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked DeleteUser({0}) method.", userName), e);
                }
            }
        }

        public int GetNumberOfUsersOnline(DateTime compareTime)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);

                    var numOnline = usersRepository.FilterBy(n => n.Membership.ApplicationName == _applicationName &&
                                          n.Membership.LastActivityDate > compareTime).Count();

                    return numOnline;
                }
                catch (Exception e)
                {
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked GetNumberOfUsersOnline({0}) method.", compareTime), e);
                }
            }
        }

        public void LockOutUser(string userName, DateTime lastLockedOutDate)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.IsLockedOut = true;
                    user.Membership.LastLockedOutDate = lastLockedOutDate;
                    unitOfWork.Commit();                    
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked LockOutUser({0}) method.", userName), e);
                }
            }
        }

        public void UnLockUser(string userName, DateTime lastLockedOutDate)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.IsLockedOut = false;
                    user.Membership.LastLockedOutDate = lastLockedOutDate;

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked UnLockUser({0}) method.", userName), e);
                }
            }
        }

        public void ApprooveUser(string userName, bool isApproved)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.IsApproved = isApproved;

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked ApprooveUser({0}) method.", userName), e);
                }
            }
        }

        public string GetUserNameByEmail(string email)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);

                    return usersRepository.FindBy(
                        n => n.Membership.Email == email && n.Membership.ApplicationName == _applicationName).UserName;
                }
                catch (Exception e)
                {
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked GetUserNameByEmail({0}) method.", email), e);
                }
            }
        }

        public void UpdateLastUserLoginDate(string userName, DateTime lastLoginDate)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.LastLoginDate = lastLoginDate;

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked UpdateLastUserLoginDate({0}) method.", userName), e);
                }
            }
        }


        public void UpdateLastUserActivityDate(string userName, DateTime time)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.LastActivityDate = time;

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked UpdateLastUserActivityDate({0}) method.", userName), e);
                }
            }
        }


        public void UpdateUser(string userName, string email, bool isApproved)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    var usersRepository = _usersRepositoryFactory.CreateUsersRepository(unitOfWork);
                    var user = usersRepository.FindBy(n => n.UserName == userName && n.Membership.ApplicationName == _applicationName);
                    user.Membership.Email = email;
                    user.Membership.IsApproved = isApproved;

                    unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw new MembershipServiceException(
                        string.Format("An error occurred when invoked UpdateLastUserActivityDate({0}) method.", userName), e);
                }
            }
        }
    }
}
