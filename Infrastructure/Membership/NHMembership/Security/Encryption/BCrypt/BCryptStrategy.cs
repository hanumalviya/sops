using bc = BCrypt.Net;

namespace NHMembership.Security.Encryption.BCrypt
{
    public class BCryptStrategy : IEncryptionStrategy
    {
        public string Encrypt(string value, out string salt)
        {
            salt = bc.BCrypt.GenerateSalt();

            return bc.BCrypt.HashPassword(value, salt);
        }

        public bool Verify(string value, string encryptedValue, string salt)
        {
            string hash = bc.BCrypt.HashPassword(value, salt);

            return hash == encryptedValue;
        }
    }
}