using System;
using System.Text.RegularExpressions;

namespace MailBot.Client.Validation
{
    /// <summary>
    ///     Defines rules for possible user inputs and validates resources to match these rules.
    /// </summary>
    public static class UserInputValidator
    {
        //could have been much more restricting and error-prone.
        private static readonly Regex EmailRegex = new Regex( @"^([^@]+@[^@]+)$", RegexOptions.Multiline | RegexOptions.Compiled );
        /*
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
         * */

        public static void AssertMailBox(string mailbox)
        {
            if (string.IsNullOrWhiteSpace( mailbox ))
            {
                throw new ArgumentException( "mailbox" );
            }
            mailbox = mailbox.Trim();
            var match = EmailRegex.Match( mailbox );
            if (!( match.Success && match.Length == mailbox.Length ))
            {
                throw new ArgumentException( "Has to be an email in [name]@[host] format." );
            }
        }

        /// <exception cref="ArgumentException">Username doesn't match the rules.</exception>
        public static void AssertUsernameRule(string username)
        {
            if (string.IsNullOrWhiteSpace( username ))
            {
                throw new ArgumentException( "Shouldn't be empty string." );
            }
            /*
            if (string.IsNullOrWhiteSpace( username )
                || !InRange( UsernameMinimumSymbols, username.Length, UsernameMaximumSymbols )
                || !ValidateFullMatch( username, IdentifierRegex ))
            {
                throw new ArgumentException( UserNameRule );
            }*/
        }

        /// <exception cref="ArgumentException">Password doesn't match the rules.</exception>
        public static void AssertPasswordRule(string password)
        {
            if (string.IsNullOrWhiteSpace( password ))
            {
                throw new ArgumentException( "Shouldn't be empty string." );
            }
            /*
            if (string.IsNullOrWhiteSpace( password )
                || !InRange( PasswordMinimumSymbols, password.Length, PasswordMaximumSymbols )
                || !ValidateFullMatch( password, IdentifierRegex )
                || !ValidateMatch( password, LetterRegex )
                || !ValidateMatch( password, DigitRegex ))
            {
                throw new ArgumentException( PasswordRule );
            }*/
        }
    }
}