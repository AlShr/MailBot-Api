using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using MailBot.Security.Background;

namespace MailBot.Security
{
    public static class Encryptor
    {
        #region byte work

        private static readonly byte[] DefaultSalt = { 13, 47, 14, 1, 81, 5, 61, 90, 99, 0 };
        private static readonly string DefaultSaltString = "salamAleiqum";

        private static readonly string DefaultKey = "g0pyLz18VjpjnMkngMHnoDY9sQQ71rJobDBIYQtMA1299I+eL0zzVUubv3ZnPwGD"
                                                    + "JbAw3LOyQEUU/kmTK0nyaH7k6IhCJjyQa+zZv31XL0Ij8pdL6+1mvoLu06GmNJIH"
                                                    + "SB/M9QjIVdgC5/dqxKy2Uqu9rkjr4/kNJmvQOPv1aK73MT7i2AJSfoCZpCVbO8i1"
                                                    + "idMQ001FnYsrbM+oYVS8lp5gHTKba9cnNXePw6irGWdf7EeFo0a7HuDjD3KNSnRJ";

        private static byte[] EncryptBytes(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            return EncryptBytes( bytesToBeEncrypted, passwordBytes, DefaultSalt );
        }

        private static byte[] EncryptBytes(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] salt)
        {
            var saltedBytes = AddSalt( bytesToBeEncrypted, 32 );
            byte[] encryptedBytes;
            var key = new Rfc2898DeriveBytes( passwordBytes, salt, 1000 );
            using (var memoryStream = new MemoryStream())
            {
                using (var aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes( aes.KeySize / 8 );
                    aes.IV = key.GetBytes( aes.BlockSize / 8 );
                    aes.Mode = CipherMode.CBC;
                    using (var cryptoStream = new CryptoStream( memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write ))
                    {
                        cryptoStream.Write( saltedBytes, 0, saltedBytes.Length );
                        cryptoStream.Close();
                    }
                    encryptedBytes = memoryStream.ToArray();
                }
            }
            return encryptedBytes;
        }

        private static byte[] DecryptBytes(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            return DecryptBytes( bytesToBeDecrypted, passwordBytes, DefaultSalt );
        }

        private static byte[] DecryptBytes(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] salt)
        {
            byte[] decryptedBytes;
            var key = new Rfc2898DeriveBytes( passwordBytes, salt, 1000 );
            using (var memoryStream = new MemoryStream())
            {
                using (var aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes( aes.KeySize / 8 );
                    aes.IV = key.GetBytes( aes.BlockSize / 8 );
                    aes.Mode = CipherMode.CBC;
                    using (var cryptoStream = new CryptoStream( memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write ))
                    {
                        cryptoStream.Write( bytesToBeDecrypted, 0, bytesToBeDecrypted.Length );
                        cryptoStream.Close();
                    }
                    decryptedBytes = memoryStream.ToArray();
                }
            }
            return StripSalt( decryptedBytes, 32 );
        }

        private static byte[] AddSalt(byte[] bytesToBeEncrypted, int count)
        {
            var randomBytes = RandomBytesGenerator.Get( count );
            var array = randomBytes.Concat( bytesToBeEncrypted ).ToArray();
            return array;
        }

        private static byte[] StripSalt(byte[] bytesToBeDecrypted, int count)
        {
            var array = bytesToBeDecrypted.Skip( count ).ToArray();
            return array;
        }

        #endregion

        #region string manipulation interface

        public static string EncryptString(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            return EncryptString( input, DefaultKey, DefaultSaltString );
        }

        public static string EncryptString(string input, string password)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            if (password == null)
            {
                throw new ArgumentNullException( "password" );
            }
            return EncryptString( input, password, DefaultSaltString );
        }

        public static string EncryptString(string input, string password, string salt)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            if (password == null)
            {
                throw new ArgumentNullException( "password" );
            }
            if (salt == null)
            {
                throw new ArgumentNullException( "salt" );
            }
            var bytesToBeEncrypted = ByteConverter.StringToByteArray( input );
            var passwordBytes = ByteConverter.StringToByteArray( password );
            var saltBytes = ByteConverter.StringToByteArray( salt + DefaultSaltString );
            var hashedBytes = SHA256.Create().ComputeHash( passwordBytes );
            var bytesEncrypted = EncryptBytes( bytesToBeEncrypted, hashedBytes, saltBytes );
            var result = ByteConverter.ByteArrayToBase64String( bytesEncrypted );
            return result;
        }

        public static string DecryptString(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            return DecryptString( input, DefaultKey, DefaultSaltString );
        }

        public static string DecryptString(string input, string password)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            if (password == null)
            {
                throw new ArgumentNullException( "password" );
            }
            return DecryptString( input, password, DefaultSaltString );
        }

        public static string DecryptString(string input, string password, string salt)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            if (password == null)
            {
                throw new ArgumentNullException( "password" );
            }
            if (salt == null)
            {
                throw new ArgumentNullException( "salt" );
            }
            var bytesToBeDecrypted = Convert.FromBase64String( input );
            var passwordBytes = ByteConverter.StringToByteArray( password );
            var saltBytes = ByteConverter.StringToByteArray( salt + DefaultSaltString );
            var hashedBytes = SHA256.Create().ComputeHash( passwordBytes );
            var bytesDecrypted = DecryptBytes( bytesToBeDecrypted, hashedBytes, saltBytes );
            var result = ByteConverter.ByteArrayToUnicodeString( bytesDecrypted );
            return result;
        }

        #endregion
    }
}