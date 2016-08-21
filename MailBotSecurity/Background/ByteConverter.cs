using System;
using System.Text;

namespace MailBot.Security.Background
{
    public static class ByteConverter
    {
        public static byte[] StringToByteArray(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            return Encoding.UTF8.GetBytes( input );
        }

        public static string ByteArrayToBase64String(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            return Convert.ToBase64String( input );
        }

        public static string ByteArrayToUnicodeString(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException( "input" );
            }
            return Encoding.UTF8.GetString( input );
        }
    }
}