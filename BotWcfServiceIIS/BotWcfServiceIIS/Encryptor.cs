using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BotWcfServiceIIS
{
    public static class Encryptor
    {
        private static string salt = "thisismykewlsalt,lol12345";

        public static string Encrypt(string input)
        {
            input += salt;
            var sha = SHA512.Create();
            var inputBytes = new byte[input.Length * sizeof (char)];
            Buffer.BlockCopy(input.ToCharArray(), 0, inputBytes, 0, input.Length);
            var resultBytes = sha.ComputeHash( inputBytes );
            for (var i = 0; i < 10; ++i)
            {
                resultBytes = sha.ComputeHash( resultBytes );
            }
            return BytesToHexadecimalString(resultBytes);
        }

        private static string BytesToHexadecimalString(IEnumerable<byte> bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append( b.ToString( "x" ) );
            }
            return sb.ToString();
        }
    }
}