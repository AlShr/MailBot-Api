using System;
using System.Collections.Generic;
using System.Net.Mail;
using MailBot.Service.MailFetching.Arguments;

namespace MailBot.Service.MailFetching.Loaders
{
    public abstract class MailLoader : IDisposable
    {
        private static readonly Dictionary<MailProtocol, Func<MailboxInfo, HostInfo, MailLoader>> ProtocolToLoaderDictionary = new Dictionary<MailProtocol, Func<MailboxInfo, HostInfo, MailLoader>>
        {
            //{ MailProtocol.Pop3, (m, h) => new Pop3Loader( m, h ) },
            { MailProtocol.Imap, (m, h) => new ImapLoader( m, h ) }
        };

        /// <exception cref="ArgumentNullException"><paramref name="mailboxInfo" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hostInfo" /> is <see langword="null" />.</exception>
        protected MailLoader(MailboxInfo mailboxInfo, HostInfo hostInfo)
        {
            if (mailboxInfo == null)
            {
                throw new ArgumentNullException( "mailboxInfo" );
            }
            if (hostInfo == null)
            {
                throw new ArgumentNullException( "hostInfo" );
            }
            MailboxInfo = mailboxInfo;
            HostInfo = hostInfo;
        }

        public HostInfo HostInfo { get; private set; }
        public MailboxInfo MailboxInfo { get; private set; }

        /// <exception cref="ArgumentNullException"><paramref name="mailbox" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="host" /> is <see langword="null" />.</exception>
        public static MailLoader GetLoader(MailboxInfo mailbox, HostInfo host)
        {
            if (mailbox == null)
            {
                throw new ArgumentNullException( "mailbox" );
            }
            if (host == null)
            {
                throw new ArgumentNullException( "host" );
            }
            return ProtocolToLoaderDictionary[host.Protocol]( mailbox, host );
        }

        public abstract IEnumerable<MailMessage> GetMessages();
        public abstract int GetMessageCount();

        public virtual void Dispose()
        {
        }
    }
}