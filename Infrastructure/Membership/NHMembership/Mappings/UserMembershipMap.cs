using FluentNHibernate;
using FluentNHibernate.Mapping;
using NHMembership.Models;

namespace NHMembership.Mappings
{
    internal class UserMembershipMap : ClassMap<UserMembership>
    {
        public UserMembershipMap()
        {
            Id(m => m.Id).GeneratedBy.Foreign("UserProfile");
            HasOne(Reveal.Member<UserMembership, UserProfile>("UserProfile")).Constrained().ForeignKey();
            Map(m => m.ApplicationName);
            Map(m => m.CreationDate);
            Map(m => m.Email);
            Map(m => m.FailedPasswordAnswerAttemptCount);
            Map(m => m.FailedPasswordAnswerAttemptWindowStart);
            Map(m => m.FailedPasswordAttemptCount);
            Map(m => m.FailedPasswordAttemptWindowStart);
            Map(m => m.IsApproved);
            Map(m => m.IsLockedOut);
            Map(m => m.LastActivityDate).Nullable();
            Map(m => m.LastLockedOutDate).Nullable();
            Map(m => m.LastLoginDate).Nullable();
            Map(m => m.LastPasswordChangedDate).Nullable();
            Map(m => m.Password).Not.Nullable();
            Map(m => m.PasswordAnswer);
            Map(m => m.PasswordQuestion);
            Map(m => m.PasswordSalt);
            Map(m => m.PasswordAnswerSalt);
        }
    }
}