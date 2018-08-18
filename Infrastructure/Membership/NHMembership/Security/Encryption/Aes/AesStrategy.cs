using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NHMembership.Security.Encryption.Aes
{
    public class AesStrategy : IEncryptionStrategy
    {
        private readonly byte[] _IV;
        private readonly byte[] _key;
        private readonly Random _random;

        public AesStrategy(byte[] key, byte[] IV)
        {
            _key = key;
            _IV = IV;
            _random = new Random();
        }

        public string Encrypt(string value, out string salt)
        {
            if (value == null || value.Length <= 0)
                throw new ArgumentNullException("value");
            if (_key == null || _key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (_IV == null || _IV.Length <= 0)
                throw new ArgumentNullException("Key");

            byte[] encrypted;
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            salt = GenerateSalt();
                            swEncrypt.Write(value + salt);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public bool Verify(string value, string encryptedValue, string salt)
        {
            return value == Decrypt(encryptedValue, salt);
        }

        private string Decrypt(string value, string salt)
        {
            if (value == null || value.Length <= 0)
                throw new ArgumentNullException("value");
            if (_key == null || _key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (_IV == null || _IV.Length <= 0)
                throw new ArgumentNullException("Key");

            string plaintext = null;

            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(value)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext.Replace(salt, string.Empty);
        }

        private string GenerateSalt()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < 15; i++)
            {
                sb.Append((char) _random.Next(256));
            }

            sb.Append(DateTime.Now.Ticks);

            return sb.ToString();
        }
    }
}