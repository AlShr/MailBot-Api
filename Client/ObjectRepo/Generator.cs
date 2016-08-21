using System;
using System.Collections.Generic;
using MailBot.Client.MailBotServiceReference;

namespace MailBot.Client.ObjectRepo
{
    internal static class Generator
    {
        private static readonly Random Rng = new Random();

        public static IEnumerable<MailMessageProxy> GenerateEmails(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                yield return GenerateMailMessageProxy();
            }
        }

        public static IEnumerable<UserProxy> GenerateUsers(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                yield return GenerateUserProxy();
            }
        }

        public static IEnumerable<EmailAddressProxy> GenerateMailboxes(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                yield return GenerateMailboxProxy();
            }
        }

        private static UserProxy GenerateUserProxy()
        {
            return new UserProxy { Login = GenerateRandomString(), Password = GenerateRandomString() };
        }

        private static MailMessageProxy GenerateMailMessageProxy()
        {
            return new MailMessageProxy { Body = GenerateRandomString(), Sender = GenerateMailboxProxy(), Subject = GenerateRandomString() };
        }

        private static EmailAddressProxy GenerateMailboxProxy()
        {
            return new EmailAddressProxy { Address = GenerateRandomString() + "@" + GenerateRandomString(), Owner = GenerateUserProxy(), VerificationKey = new VerificationKeyProxy { Status = "NE" } };
        }

        private static string GenerateRandomString()
        {
            return new string( 'r', Rng.Next( 10 ) + 10 );
        }
    }
}