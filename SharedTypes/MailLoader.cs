using System;
using System.Collections.Generic;
using System.IO;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace SharedTypes {
    public class MailLoader : IDisposable {
        private readonly Pop3Client m_client;
        private readonly MailboxInfo m_mailboxInfo;
        private readonly Pop3ServerInfo m_pop3Info;

        public MailLoader(MailboxInfo mailboxInfo) {
            if ( mailboxInfo == null ) {
                throw new ArgumentNullException( "mailboxInfo" );
            }
            m_mailboxInfo = mailboxInfo;
            m_pop3Info = mailboxInfo.ServerInfo;
            m_client = new Pop3Client();

            Connect();
            if ( !m_client.Connected ) {
                throw new IOException( "Couldn't connect with provided data." );
            }
            Authenticate();
        }

        private void Disconnect() {
            m_client.Disconnect();
        }

        private void Authenticate() {
            m_client.Authenticate( m_mailboxInfo.Mailbox, m_mailboxInfo.Password );
        }

        private void Connect() {
            m_client.Connect( m_pop3Info.Hostname, m_pop3Info.Port, m_pop3Info.UsingSsl );
        }

        public IEnumerable<Message> GetMessages() {
            var count = m_client.GetMessageCount();
            for ( var i = 1; i <= count; ++i ) {
                yield return m_client.GetMessage( i );
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            m_client.Dispose();
        }

        #endregion
    }
}