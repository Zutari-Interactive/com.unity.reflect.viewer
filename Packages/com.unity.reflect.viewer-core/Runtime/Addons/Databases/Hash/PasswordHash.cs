using System;
using System.Security.Cryptography;

namespace Databases.Hash
{
    /// <summary>
    /// SQL Password Hasher.
    /// </summary>
    public class PasswordHash
    {
        #region VARIABLES

        /// <summary>
        /// DEFAULT SALT BYTE SIZE
        /// </summary>
        private const int DEFAULT_SALT_BYTE_SIZE = 32;

        /// <summary>
        /// DEFAULT HASH BYTE SIZE
        /// </summary>
        private const int DEFAULT_HASH_BYTE_SIZE = 32;

        /// <summary>
        /// DEFAULT HASH ITERATIONS
        /// </summary>
        private const int DEFAULT_HASH_ITERATIONS = 16384;

        #endregion

        #region METHODS

        /// <summary>
        /// Generate a pseudo-random Password Salt.
        /// </summary>
        /// <param name="saltByteSize"></param>
        /// <returns></returns>
        public static byte[] GenerateSalt(int saltByteSize = DEFAULT_SALT_BYTE_SIZE)
        {
            using (RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[saltByteSize];
                generator.GetBytes(salt);

                return salt;
            }
        }

        /// <summary>
        /// Compute the Hash of the Password + Salt.
        /// Implementation uses PBKDF2.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <param name="hashByteSize"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(
            string password, byte[] salt, int iterations = DEFAULT_HASH_ITERATIONS,
            int hashByteSize = DEFAULT_HASH_BYTE_SIZE)
        {
            using (Rfc2898DeriveBytes generator = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return generator.GetBytes(hashByteSize);
            }
        }

        /// <summary>
        /// Verify if the given login passwords hash value matches the password hash in the Database.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="existingSalt"></param>
        /// <param name="existingHash"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, byte[] existingSalt, byte[] existingHash)
        {
            byte[] computedHash = ComputeHash(password, existingSalt);

            return Equals(computedHash, existingHash);
        }


        /// <summary>
        /// Compare 2 Hash Values to check if they are Equal to Each Other.
        /// </summary>
        /// <param name="hashA"></param>
        /// <param name="hashB"></param>
        /// <returns></returns>
        private static bool Equals(byte[] hashA, byte[] hashB)
        {
            int hashALength = hashA.Length;
            int hashBLength = hashB.Length;

            int minHashLength = hashALength <= hashBLength ? hashALength : hashBLength;

            int xor = hashALength ^ hashBLength;

            for (int i = 0; i < minHashLength; i++)
            {
                xor |= hashA[i] ^ hashB[i];
            }

            return 0 == xor;
        }


        /// <summary>
        /// Convert a Byte Array to a String using ToBase64String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        #endregion
    }
}