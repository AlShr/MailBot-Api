using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MailBot.DataAccessLayer.DomainDAL
{
    public class MessageIdDao:IMessageIdDAL
    {
        public virtual int Id { set; get; }
        public virtual string MessageHash { set; get; }
        public virtual int Position { set; get; }
        public virtual MailDao MailMessageId { set; get; }
        public static MessageIdDao GetDaoObject(ISession session, string hash)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            if (hash == null)
            {
                throw new ArgumentNullException("hash");
            }
            return session.Query<MessageIdDao>().FirstOrDefault(x => x.MessageHash == hash);
        }
        public static MessageIdDao GetDaoObject(ISession session, int messageId)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            return session.Query<MessageIdDao>().FirstOrDefault(x => x.Id == messageId);
        }
        public virtual void SetPositionMessageId(ISession session, MailDao mailParent)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            if (mailParent == null)
            {
                throw new ArgumentNullException("mailParent");
            }            
            mailParent.MailMessageIds.Add(this);
            Position = mailParent.MailMessageIds.Count-1;
        }
        public static IEnumerable<MessageIdDao> GetChildMessageIds(ISession session, int parentId)
        {
            return session.Query<MessageIdDao>().Select(x=>x).Where(x => x.MailMessageId.Id == parentId).ToList();
        }
        public virtual void SaveMessageId(MailEntity mail, string hash)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (hash == null)
            {
                throw new ArgumentNullException("hash");
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {                
                var messageIdentity = GetDaoObject(uow.Session, hash);
                if (messageIdentity == null )
                {
                    messageIdentity = new MessageIdDao { MessageHash = hash };
                    uow.Session.Save(messageIdentity);
                    uow.TransactionalFlush();                   
                }
                var mailDao = MailDao.FindById(uow.Session, mail.Id);
                mailDao.SetMailMessageId(messageIdentity);                
                uow.Session.Save(mailDao);
                uow.TransactionalFlush();
                UpdateParentBranch(uow.Session, messageIdentity, mailDao);               
            }
        }
        public virtual void SetParentMail(int mailId)
        {
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var mail = uow.Session.Query<MailDao>().Select(x => x).Where(x => x.Id == mailId).FirstOrDefault();
                if (mail != null)
                {
                    var childrenIdentities = uow.Session.Query<MessageIdDao>().Select(x => x).Where(x => x.MailMessageId.Id == mail.Id).ToList();
                    List<MailDao> childMails = new List<MailDao>();
                    foreach (var childMessageId in childrenIdentities)
                    {
                        var mailIdentity = uow.Session.Query<MailDao>().Select(x => x).Where(x => x.MessageId == childMessageId.Id).FirstOrDefault();
                        if (mailIdentity != null)
                            childMails.Add(mailIdentity);
                    }
                    UpdateParentBranch(uow.Session, mail, childMails);
                }
            }
        }
        public static void UpdateParentBranch(ISession session, MessageIdDao messageIdentity, MailDao mail)
        {
            if (messageIdentity == null)
            {
                throw new ArgumentNullException("messageId");
            }
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            var parentMail = messageIdentity.MailMessageId;
            if (parentMail != null)
            {
                parentMail.AddChildrenMail(mail);
                session.Update(mail);
                session.BeginTransaction();
                session.Transaction.Commit();
            }
        }
        public static void UpdateParentBranch(ISession session, MailDao mail, IList<MailDao> childMails)
        {           
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (childMails == null)
            {
                throw new ArgumentNullException("childMails");
            }
            foreach(var m in childMails)
                mail.AddChildrenMail(m);
            session.Update(mail);
            session.BeginTransaction();
            session.Transaction.Commit();
        }
        public virtual void SaveMessageId(MailEntity mail, string[] hashes)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (hashes == null)
            {
                throw new ArgumentNullException("hashes");
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var mailDao = MailDao.SetMailMessageId(uow.Session, mail, hashes);
                var messageIds = MessageIdDao.GetChildMessageIds(uow.Session, mailDao.Id);
                foreach (var messageId in messageIds)
                {
                    ReferencesDao record = new ReferencesDao();
                    record.SetMailReference(uow.Session, mailDao, messageId);
                    uow.Session.Save(record);
                    uow.TransactionalFlush();                   
                }    
            }
        }
    }
}
