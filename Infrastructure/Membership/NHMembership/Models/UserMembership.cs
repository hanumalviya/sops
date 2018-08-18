using System;

namespace NHMembership.Models
{
    public class UserMembership
    {
        protected UserMembership()
        {
        }

        public UserMembership(UserProfile userProfile)
            : this()
        {
            UserProfile = userProfile;
            UserProfile.Membership = this;
        }

        public virtual int Id { get; protected set; }

        public virtual string Email { get; set; }

        public virtual string PasswordQuestion { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual bool IsLockedOut { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual DateTime? LastLoginDate { get; set; }

        public virtual DateTime? LastActivityDate { get; set; }

        public virtual DateTime? LastPasswordChangedDate { get; set; }

        public virtual DateTime? LastLockedOutDate { get; set; }

        public virtual DateTime? FailedPasswordAttemptWindowStart { get; set; }

        public virtual int FailedPasswordAttemptCount { get; set; }

        public virtual int FailedPasswordAnswerAttemptCount { get; set; }

        public virtual DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }

        public virtual string ApplicationName { get; set; }

        public virtual string Password { get; set; }

        public virtual string PasswordAnswer { get; set; }

        public virtual string PasswordSalt { get; set; }

        public virtual string PasswordAnswerSalt { get; set; }

        protected UserProfile UserProfile { get; set; }
    }
}