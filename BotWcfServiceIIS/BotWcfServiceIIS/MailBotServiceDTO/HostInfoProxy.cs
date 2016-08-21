using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class HostInfoProxy
    {
        [DataMember]
        public MailProtocolProxy Protocol { get; set; }

        [DataMember]
        public string Hostname { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public bool UsingSsl { get; set; }
    }
}