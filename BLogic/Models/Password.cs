using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BLogic.Models
{
    public static class Password
    {
        public static bool PasswordCheck(string pass, string sha)
        {
            if (sha == null || pass == null)
            {
                return false;
            }
            var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(pass);
            var hash = sha512.ComputeHash(bytes);
            return sha == hash.Aggregate("", (current, b) => current + $"{b:x2}");
        }

        public static string HashIt(string pass)
        {
            if (pass == null)
            {
                return null;
            }
            else
            {
                var sha512 = SHA512.Create();
                var bytes = Encoding.UTF8.GetBytes(pass);
                var hash = sha512.ComputeHash(bytes);
                return hash.Aggregate("", (current, b) => current + $"{b:x2}");
            }
        }
    }
}