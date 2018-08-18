using NHMembership.Models;
using System;
using System.Collections.Generic;

namespace NHMembership.Membership.Services
{
    public interface IMembershipService
    {
        IList<UserProfile> GetUsersByEmail(string email);
        IList<UserProfile> GetUsersByName(string name);
        IList<UserProfile> AllUsers();

        UserProfile GetUserProfileByName(string userName);
        UserProfile GetUserProfileByKey(int key);

        void UpdatePasswordFailureCount(string userName, int count, DateTime attemptTime);
        void UpdatePasswordAnswerFailureCount(string userName, int count, DateTime attemptTime);

        void ChangeUserPassword(string userName, string password, string passwordSalt, DateTime lastPasswordChangedDate);
        void ChangeUserQuestionAndAnswer(string userName, string newQuestion, string answer, string answerSalt);

        void CreateUser(string userName,
                        string password,
                        string passwordSalt,
                        string email,
                        string passwordQuestion,
                        string passwordAnswer,
                        string passwordAnswerSalt,
                        bool isApproved,
                        DateTime creationDate);

        void DeleteUser(string userName);
        int GetNumberOfUsersOnline(DateTime compareTime);

        void LockOutUser(string userName, DateTime lastLockedOutDate);
        void UnLockUser(string userName, DateTime lastLockedOutDate);
        void ApprooveUser(string userName, bool isApproved);
        string GetUserNameByEmail(string email);
        void UpdateLastUserLoginDate(string userName, DateTime time);
        void UpdateLastUserActivityDate(string userName, DateTime time);

        void UpdateUser(string userName, string email, bool isApproved);
    }
}
