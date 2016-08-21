using System;
using System.Security.Cryptography;

namespace MailBot.Security.Background
{
    internal static class RandomBytesGenerator
    {
        internal static readonly int DefaultCount = 32;
        internal static readonly RandomNumberGenerator Generator = RandomNumberGenerator.Create();

        public static byte[] Get(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException( "count < 0" );
            }
            var bytes = new byte[count];
            Generator.GetBytes( bytes );
            return bytes;
        }

        public static byte[] Get()
        {
            return Get( DefaultCount );
        }
    }
}