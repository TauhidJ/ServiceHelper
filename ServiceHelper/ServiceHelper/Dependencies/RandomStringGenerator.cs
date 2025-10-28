using System.Security.Cryptography;
using System.Text;

namespace ServiceHelper.Dependencies
{
    public class RandomStringGenerator
    {
        public static string GenerateHex(int length = 32)
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();

            byte[] str = new byte[length];
            randomNumberGenerator.GetBytes(str);

            StringBuilder stringBuilder = new();
            for (int i = 0; i < str.Length; i++)
            {
                stringBuilder.Append(str[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
