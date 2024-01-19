using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Domain.Entitites;

namespace Assist.Lunch._4.Core.Helpers.Encrypter
{
    public interface IPasswordManager
    {
        public string Encrypt(string plainText);
        public string Decrypt(string encryptedText);
        public bool PasswordCheck(string encryptedRequest, string storedEncryptedText);
        public User UpdatePassword(User user, UpdateUserPasswordDto updatePasswordDto);
    }
}
