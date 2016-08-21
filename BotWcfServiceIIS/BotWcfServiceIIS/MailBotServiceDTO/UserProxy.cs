using System.Runtime.Serialization;

namespace MailBot.Service.MailBotServiceDTO
{
    [DataContract]
    public class UserProxy
    {
        [DataMember]
        public virtual int Id { get; set; }

        [DataMember]
        public virtual string Login { get; set; }

        [DataMember]
        public virtual string Password { get; set; }
    }
}