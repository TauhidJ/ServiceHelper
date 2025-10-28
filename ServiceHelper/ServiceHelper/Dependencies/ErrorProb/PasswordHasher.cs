using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace ServiceHelper.Dependencies.ErrorProb
{
    public class PasswordHasher
    {
        private readonly int _saltSize = 128 / 8;
        private readonly int _hashSize = 256 / 8;
        private readonly int _iterationCount = 10000;
        private readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();
        private readonly KeyDerivationPrf _keyDerivationPrf = KeyDerivationPrf.HMACSHA512;

        /// <summary>
        /// Hash <paramref name="password"/>
        /// </summary>
        /// <param name="password">Password that needed to be hashed.</param>
        /// <returns>Hashed password string</returns>
        public string Hash(string password)
        {
            byte[] salt = new byte[_saltSize];
            _randomNumberGenerator.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, _keyDerivationPrf, _iterationCount, _hashSize);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker; there is no need for it in this case because it is used by Identity to identify version (V2 or V3).
            WriteNetworkByteOrder(outputBytes, 1, (uint)_keyDerivationPrf);
            WriteNetworkByteOrder(outputBytes, 5, (uint)_iterationCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)_saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + _saltSize, subkey.Length);
            return Convert.ToBase64String(outputBytes);
        }

        /// <summary>
        /// Verify the <paramref name="providedPassword"/> by checking it against <paramref name="hashedPassword"/>.
        /// </summary>
        /// <param name="hashedPassword">Hashed string of the original password.</param>
        /// <param name="providedPassword">Password string provided by the end user.</param>
        /// <returns><c>true</c> if password matches, other <c>false</c>.</returns>
        public bool Verify(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (providedPassword == null) throw new ArgumentNullException(nameof(providedPassword));

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);
            if (decodedHashedPassword.Length == 0) return false;

            try
            {
                // Read header information
                KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
                int iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);
                int saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }
                byte[] salt = new byte[saltLength];
                Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                int subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }
                byte[] expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                // Hash the incoming password and verify it
                byte[] actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterCount, subkeyLength);

                return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return (uint)buffer[offset + 0] << 24
                | (uint)buffer[offset + 1] << 16
                | (uint)buffer[offset + 2] << 8
                | buffer[offset + 3];
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }
    }
}
