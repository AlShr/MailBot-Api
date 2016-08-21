using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class MailboxInfoProxy
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}