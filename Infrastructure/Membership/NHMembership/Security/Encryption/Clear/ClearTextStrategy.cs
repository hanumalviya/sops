namespace NHMembership.Security.Encryption.Clear
{
    public class ClearTextStrategy : IEncryptionStrategy
    {
        public string Encrypt(string value, out string salt)
        {
            salt = string.Empty;
            return value;
        }


        public bool Verify(string value, string encryptedValue, string salt)
        {
            return value == encryptedValue;
        }

        public string Decrypt(string value, string salt)
        {
            return value;
        }
    }
}