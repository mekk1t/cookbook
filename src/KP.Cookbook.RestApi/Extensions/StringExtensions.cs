using System.Security.Cryptography;
using System.Text;

namespace KP.Cookbook.RestApi.Extensions
{
    public static class StringExtensions
    {
        public static string Sha256Hash(this string str)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sb = new();
            for (int i = 0; i < bytes.Length; i++)
                sb.Append(bytes[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
