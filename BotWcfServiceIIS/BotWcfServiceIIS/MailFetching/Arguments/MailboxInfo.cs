using System;

namespace MailBot.Service.MailFetching.Arguments
{
    public class MailboxInfo
    {
        /// <exception cref="ArgumentNullException"><paramref name="password" /> is <see langword="null" />.</exception>
        /// ///
        /// <exception cref="ArgumentNullException"><paramref name="login" /> is <see langword="null" />.</exception>
        public MailboxInfo(string login, string password)
        {
            if (string.IsNullOrWhiteSpace( login ))
            {
                throw new ArgumentNullException( "login" );
            }
            if (password == null)
            {
                throw new ArgumentNullException( "password" );
            }
            Login = login;
            Password = password;
        }

        public string Login { get; private set; }
        public string Password { get; private set; }
    }
}