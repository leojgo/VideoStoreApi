using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VideoStoreApi.Utils
{
    public class PasswordUtils
    {
        /*
         * Crypto Code Copied / Modified From: https://stackoverflow.com/questions/4181198/how-to-hash-a-password/10402129#10402129
         * Regex  Code Copied / Modified From: https://msdn.microsoft.com/en-us/library/ff648339.aspx
         */

        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int MinPwLength = 6;
        private const int MaxPwLength = 16;
        private const int MinStringLength = 1;
        private const int MaxStringLength = 25;


        private static string Hash(string password, int iterations)
        {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            //format hash with extra information
            return $"$LACKLUSTER$V1${iterations}${base64Hash}";
        }

        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$LACKLUSTER$V1$");
        }

        public static bool Verify(string password, string hashedPassword)
        {
            //check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            //extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$LACKLUSTER$V1$", "").Split('$');
            var iterations = Int32.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //get hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsPasswordFormatValid(string password)
        {
            /*
             * Requirements for Passwords:
             * At Least 1 Digit
             * At Least 1 Uppercase Letter
             * At Least 1 Lowercase Letter
             * Length is Greater than MinPwLength (6)
             * Length is Less than MaxPwLength (25)
             */
            return Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{" + MinPwLength + "," + MaxPwLength + "}$");
        }

        public static bool IsNameFormatValid(string inputString)
        {
            /*
             * Requirements for other Database Entries (Names, Addresses, etc):
             * Can Only contain the following:
             * Uppercase Letters
             * Lowercase Letters
             * '.' (Period)
             * '-' (Dash)
             * ' ' (White Space)
             * ''' (Apostrophe)
             * '@' (At symbol)
             *
             * AND
             *
             * Length is Greater than MinStringLength (1)
             * Length is Less than MaxStringLength (25)
             */
            return Regex.IsMatch(inputString, @"^[a-zA-Z'.-@/s]{" + MinStringLength + "," + MaxStringLength + "}$");
        }
    }
} 
