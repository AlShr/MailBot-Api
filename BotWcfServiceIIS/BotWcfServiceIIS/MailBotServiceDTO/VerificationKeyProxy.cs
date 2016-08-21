using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class VerificationKeyProxy
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string Key { get; set; }

        [DataMember]
        public virtual string Status { get; set; }

        [DataMember]
        public virtual EmailAddressProxy EmailAdress { get; set; }
    }
}