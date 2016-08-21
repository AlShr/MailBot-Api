using System;
using System.Net;
using System.Net.Mail;
using MailBot.Service.Converter;
using MailBot.Service.MailBotServiceDTO;
using MailBot.Service.MailFetching.Arguments;

namespace MailBot.Service.MailFetching
{
    public class SmtpWrapper
    {
        public SmtpWrapper(MailboxInfoProxy mailbox, HostInfoProxy host)
            : this( PolymorphConverter.ToRepoItem( mailbox ), PolymorphConverter.ToRepoItem( host ) )
        {
        }

        public SmtpWrapper(MailboxInfo mailbox, HostInfo host)
        {
            Mailbox = mailbox;
            Host = host;
        }

        public MailboxInfo Mailbox { get; private set; }
        public HostInfo Host { get; private set; }

        private SmtpClient Get()
        {
            return new SmtpClient( Host.Hostname, Host.Port )
            {
                Credentials = new NetworkCredential( Mailbox.Login, Mailbox.Password ),
                EnableSsl = Host.UsingSsl
            };
        }

        public void SendMessage(MailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException( "message" );
            }
            using (var client = Get())
            {
                message.Sender = message.Sender ?? new MailAddress( BotMailbox.GetImapInstance().MailboxInfo.Login );
                client.Send( message );
            }
        }
    }
}