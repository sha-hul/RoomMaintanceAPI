using System.Security.Cryptography;
using System.Text;

namespace RoomMaintenanceAPI.Models
{
    public static class CryptoHelper
    {
        // Change these and keep them SAFE (prefer appsettings.json or KeyVault)
        private static readonly string EncryptionKey = "MJFISKND-sjNSjSnsmja324-ff";
        private static readonly string Salt = "In0fnwkq3NJAO23as3E";
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(
                    EncryptionKey,
                    Encoding.UTF8.GetBytes(Salt),
                    10000,
                    HashAlgorithmName.SHA256
                );
                aes.Key = key.GetBytes(32); // 256-bit key
                aes.GenerateIV(); // Random IV
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    // Store IV at the beginning
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(
                    EncryptionKey,
                    Encoding.UTF8.GetBytes(Salt),
                    10000,
                    HashAlgorithmName.SHA256
                );
                aes.Key = key.GetBytes(32);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                // Extract IV
                byte[] iv = new byte[16];
                Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(
                        new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length),
                        decryptor,
                        CryptoStreamMode.Read))
                    {
                        cs.CopyTo(ms);
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

    }
}
