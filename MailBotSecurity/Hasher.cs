using System;
using System.Security.Cryptography;
using MailBot.Security.Background;

namespace MailBot.Security
{
    public static class Hasher
    {
        private static readonly string Salt = "mykewlsalt12345_.:";
        private static readonly SHA256 ShaInstance = SHA256.Create();

        private static byte[] EncryptBytes(byte[] bytes)
        {
            return ShaInstance.ComputeHash( bytes );
        }

        public static string Encrypt(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            var bytes = ByteConverter.StringToByteArray( string.Format( "{0}{1}{0}", Salt, input ) );
            var hashedBytes = EncryptBytes( bytes );
            var count = 13 * 17 + 19;
            for (var i = 1; i < count; ++i)
            {
                hashedBytes = EncryptBytes( hashedBytes );
            }
            return Convert.ToBase64String( hashedBytes );
        }
    }
}