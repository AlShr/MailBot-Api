using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class AttachmentProxy
    {
        [DataMember]
        public virtual int Id { get; protected set; }

        [DataMember]
        public virtual byte[] Contents { get; set; }

        [DataMember]
        public virtual MailMessageProxy Mail { get; set; }
    }
}