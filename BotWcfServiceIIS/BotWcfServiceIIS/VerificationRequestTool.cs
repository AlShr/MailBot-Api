using System;
using System.Net.Mail;
using System.Text;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.Service.MailFetching;

namespace MailBot.Service
{
    public static class VerificationRequestTool
    {
        private static readonly Random Random = new Random( DateTime.Now.Millisecond );

        #region Main functions (public)

        /// <summary>
        ///     Initiate email verification:generaty code, crate message, send message and create record with email-code pair.
        /// </summary>
        /// <param name="email">target email</param>
        /// <param name="username">user login</param>
        /// <exception cref="ArgumentNullException"><paramref name="email" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="username" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">This email already exist in base</exception>
        public static VerificationKeyEntity InitiateEmailAddressVerification(string email, string username, string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            if (username == null)
            {
                throw new ArgumentNullException( "username" );
            }
            IEmailAddressDAL emailAddressDAL = new EmailAddressDao();
            if (!emailAddressDAL.IsEmailAddressExists( email ))
            {
                throw new ArgumentException( "This email does not exist in base." );
            }
            var code = GenerateCode();
            var message = ConstructMessage( email, code );
            BotMailbox.GetSmtpInstance().SendMessage( message );
            return CreateVerificationEmailRecords( email, code, username, password );
        }

        /// <exception cref="ArgumentNullException"><paramref name="email" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="groupName" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">This email already exist in base</exception>
        public static VerificationKeyEntity InitiateEmailAddressVerification(string email, string groupName)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }
            if (groupName == null)
            {
                throw new ArgumentNullException("username");
            }
            IEmailAddressDAL emailAddressDAL = new EmailAddressDao();
            if (!emailAddressDAL.IsEmailAddressExists(email))
            {
                throw new ArgumentException("This email does not exist in base.");
            }
            var code = GenerateCode();
            var message = ConstructMessage(email, code);
            return CreateVerificationEmailRecords(email, code, groupName);
        }

        /// <exception cref="ArgumentNullException"><paramref name="email" /> is <see langword="null" />.</exception>
        public static bool ValidateEmailStatus(string email, string code)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            if (CompareCodes( email, code ) == false)
            {
                return false;
            }
            IVerificationKeyDAL vKeyDAL = new VerificationKeyDao();
            vKeyDAL.ValidateEmailAddress( email );
            return true;
        }

        #endregion

        #region Service functions (private)

        /// <exception cref="ArgumentNullException"><paramref name="email" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="code" /> is <see langword="null" />.</exception>
        private static MailMessage ConstructMessage(string email, string code)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            if (code == null)
            {
                throw new ArgumentNullException( "code" );
            }
            var to = new MailAddress( email );
            var from = new MailAddress( "noreply@example.com" );
            var resultMessage = new MailMessage { To = { to }, From = from };
            var subject = "MailBot verification message";
            var body = "This is verification message.\nYour code: " + code
                       + ". Please enter it in your client.\nPlease dont reply on this message.";
            resultMessage.Subject = subject;
            resultMessage.Body = body;
            return resultMessage;
        }

        /// <summary>
        ///     Generates verification code (up\down case letters & digits & symvols)
        /// </summary>
        /// <param name="length">Verification code length</param>
        /// <returns>Code of a given length</returns>
        /// <exception cref="ArgumentException">length cannot be less than one</exception>
        private static string GenerateCode(int length = 16)
        {
            if (length < 1)
            {
                throw new ArgumentException( "length cannot be less than one" );
            }
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                sb.Append( allowedChars[Random.Next( allowedChars.Length - 1 )] );
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Creates record in Verification table and
        /// </summary>
        /// <param name="login">Username</param>
        /// <param name="email">Email field</param>
        /// <param name="code">Code field</param>
        /// !!!!!! moved to VerificationKeyDao
        private static VerificationKeyEntity CreateVerificationEmailRecords(string email, string code, string username, string password)
        {
            IUserDAL userDAL = new UserDao();
            IVerificationKeyDAL vKeyDAL = new VerificationKeyDao();
            var user = userDAL.GetUserByUsername( username );
            if (user == null)
            {
                throw new ArgumentException( "user doesnt exist" );
            }
            return vKeyDAL.CreateVerificationEmailRecords( email, code );
        }

        private static VerificationKeyEntity CreateVerificationEmailRecords(string email, string code, string groupName)
        {
            IGroupDAL groupDal = new GroupDao();
            IVerificationKeyDAL vKeyDAL = new VerificationKeyDao();
            var user = groupDal.FindGroupByName(groupName);
            if (user == null)
            {
                throw new ArgumentException("user doesnt exist");
            }
            return vKeyDAL.CreateVerificationEmailRecords(email, code);
            // return VerificationKeyDao.CreateVerificationEmailRecords( email, code );
        }

        /// <summary>
        ///     Compares the code received from user and the code stored in the database.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="code">Code received from user</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="email" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="code" /> is <see langword="null" />.</exception>
        private static bool CompareCodes(string email, string code)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            if (code == null)
            {
                throw new ArgumentNullException( "code" );
            }
            IVerificationKeyDAL vKeyDAL = new VerificationKeyDao();
            var vKey = vKeyDAL.GetVerificationCodeByEmail( email );
            return ( vKey != null && vKey == code );
        }

        #endregion
    }
}