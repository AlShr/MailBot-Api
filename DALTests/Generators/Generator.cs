using System;
using System.Text;

namespace DALTests.Generators
{
    public static class Generator
    {
        public static StringBuilder GenerateEmailAddress()
        {
            string domen = "@mail.local";
            StringBuilder emailAddress = new StringBuilder(25);
            emailAddress.Append(LettersNumbers(6));
            emailAddress.Append(domen);
            return emailAddress;
        }

        public static StringBuilder LettersNumbers(int n)
        {
            Random rnd = new Random(Environment.TickCount);
            StringBuilder s = new StringBuilder(25);
            string abd = "abcdefghijklmnopqrstuvwxyz0123456789";
            while (s.Length < n)
                s.Append(abd[rnd.Next(0, 36)]);
            return s;
        }
    }
}
