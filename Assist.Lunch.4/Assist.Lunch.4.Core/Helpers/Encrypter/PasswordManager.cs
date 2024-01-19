using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Resources;
using System.Security.Cryptography;
using System.Text;

namespace Assist.Lunch._4.Core.Helpers.Encrypter
{
    public class PasswordManager : IPasswordManager
    {
        protected static string EncryptionKey = "0ram@1234xxxxxxxxxxtttttuuuuuiiiiio";

        public string Encrypt(string encryptString)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[]
                {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new();
                using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                }
                encryptString = Convert.ToBase64String(ms.ToArray());
            }

            return encryptString;
        }

        public string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new(EncryptionKey, new byte[]
                {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using MemoryStream ms = new();
                using (CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }

            return cipherText;
        }

        public bool PasswordCheck(string requestPassword, string storedEncryptedText)
        {
            var decryptedStoredPassword = Decrypt(storedEncryptedText);

            return requestPassword.Equals(decryptedStoredPassword);
        }

        public User UpdatePassword(User user, UpdateUserPasswordDto updatePasswordDto)
        {
            if (!PasswordCheck(updatePasswordDto.OldPassword, user.Password))
            {
                throw new InvalidCredentialsException(UserResources.InvalidOldPassword);
            }

            if (PasswordCheck(updatePasswordDto.NewPassword, user.Password))
            {
                throw new InvalidCredentialsException(UserResources.InvalidNewPassword);
            }

            user.Password = Encrypt(updatePasswordDto.NewPassword);

            return user;
        }
    }
}
