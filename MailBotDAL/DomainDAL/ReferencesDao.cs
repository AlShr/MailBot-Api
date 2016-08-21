using NHibernate;
using System;
namespace MailBot.DataAccessLayer.DomainDAL
{
    public class ReferencesDao
    {
        public virtual int Id { set; get; }
        public virtual MessageIdDao ChildMessageId { set; get; }
        public virtual MailDao ParentMailId { set; get; }
        public virtual MessageIdDao ParentMessageId { set; get; }
        public virtual void SetMailReference(ISession session, MailDao mail, MessageIdDao childIdentity)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (childIdentity == null)
            {
                throw new ArgumentNullException("messageIdentity");
            }
            var messageIdDao = MessageIdDao.GetDaoObject(session, mail.MessageId);
            ChildMessageId = childIdentity;
            ParentMessageId = messageIdDao;
            ParentMailId = mail;
            mail.ReferencesMailRecord = this;
        }
    }
}
