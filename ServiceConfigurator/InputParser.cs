using System;
using System.Collections.Generic;

namespace MailBot.ServiceConfigurator
{
    public static class InputParser
    {
        private static readonly Dictionary<ParseType, Func<string, bool>> ValidatorFunctionDictionary = new Dictionary<ParseType, Func<string, bool>>
        {
            { ParseType.Name, s => true },
            { ParseType.Password, s => true },
            { ParseType.Host, s => true },
            { ParseType.Port, s => ValidatePort( s ) },
            { ParseType.Bool, s => IsParseable<bool>( s, bool.TryParse ) },
        };

        private static bool ValidatePort(string s)
        {
            if (IsParseable<int>( s, int.TryParse ))
            {
                var i = int.Parse( s );
                return 0 <= i && i <= short.MaxValue;
            }
            return false;
        }

        private static bool IsParseable<T>(string input, TryParseDelegate<T> tryParseDelegateFunction)
        {
            T dummy;
            return tryParseDelegateFunction( input, out dummy );
        }

        public static string ParseOrReturn(string message, ParseType type, string errorMessage)
        {
            bool isValid;
            string input;
            do
            {
                Console.Write( message );
                input = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace( input ))
                {
                    Console.CursorTop--;
                    Console.CursorLeft = 0;
                    Console.Write( message + "No changes will be committed" );
                    Console.Write( Environment.NewLine );
                    return null;
                }
                isValid = ValidatorFunctionDictionary[type]( input );
                if (!isValid)
                {
                    Console.WriteLine( errorMessage + " Try again." );
                }
            }
            while (!isValid);
            return input;
        }

        private delegate bool TryParseDelegate<T>(string input, out T output);
    }

    public enum ParseType
    {
        Name,
        Password,
        Host,
        Port,
        Bool
    }
}