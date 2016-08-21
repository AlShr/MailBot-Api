using System.ComponentModel;

namespace MailBot.Service.MailFetching.Arguments
{
    public enum MailProtocol
    {
        [Description("POP3")] Pop3,
        [Description("IMAP")] Imap
    }
}