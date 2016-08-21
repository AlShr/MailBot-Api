using System;
using System.Collections.Generic;
using MailBot.Client.MailBotServiceReference;

namespace MailBot.Client.ServiceCalls
{
    public static class ServiceCaller
    {
        public static UserProxy Authenticate(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException( "username" );
            }
            if (password == null)
            {
                throw new ArgumentNullException( "password" );
            }
            UserProxy result = null;
            ServiceConnector.ServiceCall( client =>
            {
                var proxy = new UserProxy { Login = username, Password = password };
                result = client.AuthenticateUser( proxy );
            } );
            if (result == null)
            {
                throw new ArgumentException( "Invalid credentials." );
            }
            return result;
        }

        public static UserProxy Register(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException( "username" );
            }
            if (password == null)
            {
                throw new ArgumentNullException( "password" );
            }
            UserProxy result = null;
            ServiceConnector.ServiceCall( client =>
            {
                var proxy = new UserProxy { Login = username, Password = password };
                result = client.RegisterUser( proxy );
            } );
            if (result == null)
            {
                throw new ArgumentException( "Error while registering." );
            }
            return result;
        }

        public static IEnumerable<MailMessageProxy> GetMessagesForUser(UserProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException( "proxy" );
            }
            IEnumerable<MailMessageProxy> result = null;
            ServiceConnector.ServiceCall( client => result = client.GetAllMessagesForUser( proxy ) );
            if (result == null)
            {
                throw new ArgumentException( "Couldn't fetch messages." );
            }
            return result;
        }

        public static IEnumerable<EmailAddressProxy> GetUserMailboxes(UserProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException( "proxy" );
            }
            IEnumerable<EmailAddressProxy> result = null;
            ServiceConnector.ServiceCall( client => result = client.GetUserMailboxes( proxy ) );
            if (result == null)
            {
                throw new ArgumentException( "Couldn't fetch user mailboxes." );
            }
            return result;
        }

        public static void SendVerificationCode(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            ServiceConnector.ServiceCall( client => client.VerifyEmailAddress( App.Current.CurrentUser, email ) );
        }

        public static bool ConfirmVerificationCode(string email, string code)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            var result = false;
            ServiceConnector.ServiceCall( client => result = client.ConfirmVerificationCode( email, code ) );
            return result;
        }

        public static EmailAddressProxy AddMailBox(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            EmailAddressProxy result = null;
            ServiceConnector.ServiceCall( client => result = client.AddEmailAddress( App.Current.CurrentUser, email ) );
            if (result == null)
            {
                throw new ArgumentException( "Couldn't add user mailbox." );
            }
            return result;
        }
    }
}