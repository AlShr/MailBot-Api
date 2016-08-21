using System;
using MailBot.Service.Converter;
using MailBot.Service.MailBotServiceDTO;
using MailBot.Service.MailFetching.Arguments;
using MailBot.Service.MailFetching.Loaders;

namespace MailBot.Service.MailFetching
{
    public static class BotMailbox
    {
        private static ImapLoader ImapInstance { get; set; }
        private static SmtpWrapper SmtpInstance { get; set; }

        public static ImapLoader GetImapInstance()
        {
            if (ImapInstance == null)
            {
                throw new InvalidOperationException( "Initialize imap instance before getting it." );
            }
            return ImapInstance;
        }

        public static SmtpWrapper GetSmtpInstance()
        {
            if (SmtpInstance == null)
            {
                throw new InvalidOperationException( "Initialize smtp instance before getting it." );
            }
            return SmtpInstance;
        }

        public static void InitializeImap(MailboxInfoProxy mailbox, HostInfoProxy host)
        {
            if (ImapInstance == null)
            {
                ImapInstance = new ImapLoader( PolymorphConverter.ToRepoItem( mailbox ), PolymorphConverter.ToRepoItem( host ) );
            }
            else
            {
                throw new InvalidOperationException( "Imap instance is already initialized." );
            }
        }

        public static void InitializeImap(MailboxInfo mailbox, HostInfo host)
        {
            if (ImapInstance == null)
            {
                ImapInstance = new ImapLoader( mailbox, host );
            }
            else
            {
                throw new InvalidOperationException( "Imap instance is already initialized." );
            }
        }

        public static void InitializeSMTP(MailboxInfo mailbox, HostInfo host)
        {
            if (SmtpInstance == null)
            {
                SmtpInstance = new SmtpWrapper( mailbox, host );
            }
            else
            {
                throw new InvalidOperationException( "Smtp instance is already initialized." );
            }
        }

        public static void InitializeSMTP(MailboxInfoProxy mailbox, HostInfoProxy host)
        {
            if (SmtpInstance == null)
            {
                SmtpInstance = new SmtpWrapper( mailbox, host );
            }
            else
            {
                throw new InvalidOperationException( "Smtp instance is already initialized." );
            }
        }
    }
}