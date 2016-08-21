using System;
using System.Text.RegularExpressions;

namespace SharedTypes.Validation
{
    /// <summary>
    ///     Defines rules for possible user inputs and validates resources to match these rules.
    /// </summary>
    public static class UserInputValidator
    {
        #region Rules initialization

        static UserInputValidator()
        {
            //static constructor is necessary to guarantee rule messages are generated
            //AFTER every required parameter is initialized.
            UsernameMinimumSymbols = 6;
            UsernameMaximumSymbols = int.MaxValue; //no upper bound, yet
            UserNameRule = string.Format(
                "Usernames must only contain letters, digits, underscore and a dot symbols. "
                + "Username should be at least {0} symbols long.",
                UsernameMinimumSymbols.ToString() );

            PasswordMinimumSymbols = 6;
            PasswordMaximumSymbols = int.MaxValue; //no upper bound, yet
            PasswordRule = string.Format(
                "Passwords must contain a letter and a digit, "
                + "and they must contain only letters, digits, underscore and a dot symbols."
                + "Password length should be at least {0} symbols long.",
                PasswordMinimumSymbols.ToString() );
        }

        #endregion

        /// <exception cref="ArgumentException">Username doesn't match the rules.</exception>
        public static void AssertUsernameRule(string username)
        {
            if (string.IsNullOrWhiteSpace( username )
                || !InRange( UsernameMinimumSymbols, username.Length, UsernameMaximumSymbols )
                || !ValidateFullMatch( username, IdentifierRegex ))
            {
                throw new ArgumentException( UserNameRule );
            }
        }

        /// <exception cref="ArgumentException">Password doesn't match the rules.</exception>
        public static void AssertPasswordRule(string password)
        {
            if (string.IsNullOrWhiteSpace( password )
                || !InRange( PasswordMinimumSymbols, password.Length, PasswordMaximumSymbols )
                || !ValidateFullMatch( password, IdentifierRegex )
                || !ValidateMatch( password, LetterRegex )
                || !ValidateMatch( password, DigitRegex ))
            {
                throw new ArgumentException( PasswordRule );
            }
        }

        private static bool ValidateMatch(string input, Regex regex)
        {
            return regex.IsMatch( input );
        }

        private static bool ValidateFullMatch(string input, Regex regex)
        {
            var match = regex.Match( input );
            return match.Success && match.Length.Equals( input.Length );
        }

        private static bool InRange(int min, int number, int max)
        {
            return min <= number && number <= max;
        }

        #region Username rules

        private static readonly int UsernameMinimumSymbols;
        private static readonly int UsernameMaximumSymbols;
        private static readonly Regex IdentifierRegex = new Regex( @"^[a-zA-Z0-9_\.]+$", RegexOptions.Compiled );
        private static readonly string UserNameRule;

        #endregion

        #region Password rules

        private static readonly int PasswordMinimumSymbols;
        private static readonly int PasswordMaximumSymbols;
        private static readonly Regex LetterRegex = new Regex( "[a-zA-Z]", RegexOptions.Compiled );
        private static readonly Regex DigitRegex = new Regex( @"\d", RegexOptions.Compiled );
        private static readonly string PasswordRule;

        #endregion
    }
}