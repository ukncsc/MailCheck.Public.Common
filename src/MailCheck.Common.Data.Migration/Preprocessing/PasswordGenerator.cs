using System;

namespace MailCheck.Common.Data.Migration.Preprocessing
{
    public interface IPasswordGenerator
    {
        string GeneratePassword();
    }

    public class PasswordGenerator : IPasswordGenerator
    {
        private readonly Random _random = new Random();

        public string GeneratePassword()
        {
            byte[] bytes = new byte[16];
            _random.NextBytes(bytes);
            return Convert.ToBase64String(bytes).Substring(0, 16);
        }
    }
}