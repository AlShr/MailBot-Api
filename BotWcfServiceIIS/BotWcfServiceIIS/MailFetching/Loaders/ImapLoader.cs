using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using MailBot.Service.MailFetching.Arguments;
using S22.Imap;
using MailboxInfo = MailBot.Service.MailFetching.Arguments.MailboxInfo;

namespace MailBot.Service.MailFetching.Loaders
{
    public sealed class ImapLoader : MailLoader
    {
        private readonly ImapClient client;

        public ImapLoader(MailboxInfo mailboxInfo, HostInfo hostInfo)
            : base( mailboxInfo, hostInfo )
        {
            client = new ImapClient( HostInfo.Hostname, HostInfo.Port, MailboxInfo.Login, MailboxInfo.Password, AuthMethod.Auto, HostInfo.UsingSsl );
        }

        public event EventHandler<IdleMessageEventArgs> OnNewMessage
        {
            add { client.NewMessage += value; }
            remove { client.NewMessage -= value; }
        }

        public override IEnumerable<MailMessage> GetMessages()
        {
            return client.Search( SearchCondition.All() ).Reverse().Select( messageUid => client.GetMessage( messageUid ) );
        }

        public MailMessage GetMessage(uint uid)
        {
            return client.GetMessage( uid, false );
        }

        public override int GetMessageCount()
        {
            return client.GetMailboxInfo().Messages;
        }

        public override void Dispose()
        {
            client.Dispose();
            base.Dispose();
        }
    }
}