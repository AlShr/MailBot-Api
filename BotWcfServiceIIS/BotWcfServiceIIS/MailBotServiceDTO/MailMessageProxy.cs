using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class MailMessageProxy //0
    {
        public MailMessageProxy()
        {
            Recipients = new HashSet<EmailAddressProxy>();
            Attachments = new HashSet<AttachmentProxy>();
        }

        [DataMember]
        public virtual int Id { get; protected set; }

        [DataMember]
        public virtual string Subject { get; set; }

        [DataMember]
        public virtual string Body { get; set; }

        [DataMember]
        public virtual EmailAddressProxy Sender { get; set; }

        [DataMember]
        public virtual HashSet<EmailAddressProxy> Recipients { get; set; }

        [DataMember]
        public virtual HashSet<AttachmentProxy> Attachments { get; set; }
    }
}