using System.Security.Cryptography;
using System.Text;

namespace RiskDashBoard.Resources
{
    public static class EncryptFunctions
    {
        public static string Sha256Converter(string text)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(text));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
