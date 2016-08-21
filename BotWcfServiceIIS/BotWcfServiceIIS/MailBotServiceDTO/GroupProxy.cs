using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class GroupProxy
    {
        

        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string GroupName { get; set; }

        [DataMember]
        public virtual EmailAddressProxy GroupAddress { get; set; }
    }
}