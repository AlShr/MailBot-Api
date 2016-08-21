using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class EmailAddressProxy
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string Address { get; set; }

        [DataMember]
        public virtual UserProxy Owner { get; set; }

        [DataMember]
        public virtual GroupProxy GroupOwner { get; set; }

        [DataMember]
        public virtual VerificationKeyProxy VerificationKey { get; set; }
    }
}