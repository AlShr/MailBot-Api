

namespace MailBot.DataAccessLayer.BusinessLayer.BusinessEntity
{
    public class MessageIdEntity
    {
        public virtual int Id { set; get; }
        public virtual string MessageHash { set; get; }
        public virtual int Position { set; get; }
        public virtual MailEntity MailMessageId { set; get; }
    }
}
