using System;
using System.Collections.Generic;
using System.Linq;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using NHibernate;
using NHibernate.Linq;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public class MailDao : IMailDAL
    {
        public virtual int Id { get; protected set; }
        public virtual int? ParentId { get; set; }
        public virtual MailDao Parent { get; set; }        
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual EmailAddressDao Sender { get; set; }
        public virtual ISet<EmailAddressDao> Recipients { get; protected set; }
        public virtual ISet<AttachmentDao> Attachments { get; protected set; }
        public virtual ISet<MailDao> Children { get; set; }
        public virtual IList<MessageIdDao> MailMessageIds { get; set; }
        public virtual int MessageId { set; get; }       
        public virtual ReferencesDao ReferencesMailRecord { set; get; }
        public MailDao()
        {
            Recipients = new HashSet<EmailAddressDao>();
            Attachments = new HashSet<AttachmentDao>();
            Children = new HashSet<MailDao>();
            MailMessageIds = new List<MessageIdDao>();            
        }
        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <see langword="null" />.</exception>
        public MailDao(EmailAddressDao user)
        {
            Recipients = new HashSet<EmailAddressDao>();
            Attachments = new HashSet<AttachmentDao>();
            Children = new HashSet<MailDao>();
            MailMessageIds = new List<MessageIdDao>();
            if (user == null)
            {
                throw new ArgumentNullException( "user" );
            }
            user.Mails.Add( this );
            Sender = user;
        }
        public static MailDao GetMailByMessageId(ISession session, string hash)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            if (hash == null)
            {
                throw new ArgumentNullException("hash");
            }
            var messageIdDao = MessageIdDao.GetDaoObject(session, hash);
            return messageIdDao != null 
                ?session.Query<MailDao>().FirstOrDefault(mail => mail.MessageId == messageIdDao.Id)
                :null;
        }
        public virtual void SetParentMail(MailDao mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            Parent = mail;
        }
        public virtual void AddChildrenMail(MailDao mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException("mail");
            }
            if (!Children.Contains(mail))
            {
                SetParentMail(this);               
                Children.Add(mail);
            }           
        }
        public virtual void SetMailMessageId(MessageIdDao messageId)
        {
            if (messageId == null)
            {
                throw new ArgumentNullException("messageId");
            }
            MessageId = messageId.Id;           
        }
        public virtual MailEntity SetMailMessageId(MailEntity mail, string[] hashes)
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
                var mailDao = FindById(uow.Session, mail.Id);
                foreach (var hash in hashes)
                {
                    var messageIdentity = MessageIdDao.GetDaoObject(uow.Session, hash);
                    if (messageIdentity == null)
                    {
                        messageIdentity = new MessageIdDao { MessageHash = hash };
                    }
                    messageIdentity.MailMessageId = mailDao;
                    messageIdentity.SetPositionMessageId(uow.Session, mailDao);
                    uow.Session.Save(messageIdentity);
                    uow.TransactionalFlush();                    
                }
                return MailEntity.ToBusinessItem(mailDao);
            }
        }
        public static MailDao SetMailMessageId(ISession session,MailEntity mail, string[] hashes)
        {
            if (hashes == null)
            {
                throw new ArgumentNullException("hashes");
            }
            var mailDao = FindById(session, mail.Id);
            foreach (var hash in hashes)
            {
                var messageIdentity = MessageIdDao.GetDaoObject(session, hash);
                if (messageIdentity == null)
                {
                    messageIdentity = new MessageIdDao { MessageHash = hash };
                }
                messageIdentity.MailMessageId = mailDao;
                messageIdentity.SetPositionMessageId(session, mailDao);
                session.Save(messageIdentity);
                session.BeginTransaction();
                session.Transaction.Commit();               
            }
            return mailDao;
        }
        public virtual void AddSender(EmailAddressDao sender)
        {
            if (sender == null)
            {
                throw new ArgumentNullException( "sender" );
            }
            sender.Mails.Add( this );
            Sender = sender;
        }

        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <see langword="null" />.</exception>
        public virtual void AddRecipient(EmailAddressDao email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }

            if (!Recipients.Contains( email ))
            {
                email.AddMail( this );
                Recipients.Add( email );
            }
        }

        public virtual void AddAtachment(AttachmentDao attachment)
        {
            if (attachment == null)
            {
                throw new ArgumentNullException( "attachment" );
            }
            if (!Attachments.Contains( attachment ))
            {
                attachment.AddMails( this );
                Attachments.Add( attachment );
            }
        }      
        public static MailDao FindById(ISession session, int id)
        {
            if (session == null)
            {
                throw new ArgumentNullException( "session" );
            }
            return session.Query<MailDao>().FirstOrDefault( mail => mail.Id == id );
        }

        public virtual MailEntity AddMail(string subject, string body, EmailAddressEntity sender)
        {
            if (subject == null)
            {
                throw new ArgumentNullException( "subject" );
            }
            if (body == null)
            {
                throw new ArgumentNullException( "body" );
            }
            if (sender == null)
            {
                throw new ArgumentNullException( "sender" );
            }
            var mail = new MailDao { Subject = subject, Body = body };
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var emailDao = EmailAddressDao.GetDaoObject( uow.Session, sender.Address );
                mail.AddSender( emailDao );
                uow.Session.Save( mail );
                uow.TransactionalFlush();                
                return MailEntity.ToBusinessItem( mail );
            }
        }

        public virtual MailEntity FindById(int id)
        {
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                if (uow == null)
                {
                    throw new InvalidOperationException( "UnitOfWork is null" );
                }
                var findedMessage = MailEntity.ToBusinessItem(
                    uow.Session.Query<MailDao>().FirstOrDefault( mail => mail.Id == id ) );
                return findedMessage;
            }
        }
        public static MailDao GetMailByMessageId(ISession session, MessageIdDao messageId)
        {
            if (messageId == null)
            {
                throw new ArgumentNullException("messageId");
            }
            return messageId.MailMessageId != null ? MailDao.FindById(session, messageId.MailMessageId.Id)
                : null;
        }
    }
}
