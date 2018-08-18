namespace NHMembership.Security.Encryption
{
    public interface IEncryptionStrategy
    {
        string Encrypt(string value, out string salt);
        bool Verify(string value, string encryptedValue, string salt);
    }
}