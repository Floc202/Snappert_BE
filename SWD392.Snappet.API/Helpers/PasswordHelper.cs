namespace SWD392.Snappet.API.Helpers
{
    using System.Security.Cryptography;

    public class PasswordHelper
    {
        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            var saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hash = pbkdf2.GetBytes(20);
            string hashedPassword = Convert.ToBase64String(hash);

            return (hashedPassword, salt);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hash = pbkdf2.GetBytes(20);

            return Convert.ToBase64String(hash) == storedHash;
        }
    }

}
