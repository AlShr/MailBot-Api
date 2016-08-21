using System.ComponentModel;
using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract(Name = "MailProtocol")]
    public enum MailProtocolProxy
    {
        [EnumMember, Description("POP3")] Pop3,
        [EnumMember, Description("IMAP")] Imap
    }
}