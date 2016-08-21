using System;
using System.Collections.Generic;
using System.Linq;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using NHibernate;
using NHibernate.Linq;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public class EmailAddressDao : IEmailAddressDAL
    {
        public EmailAddressDao()
        {
            RecipeMails = new HashSet<MailDao>();
            Mails = new HashSet<MailDao>();
        }
        public virtual int Id { get; protected set; }
        public virtual string Address { get; set; }
        public virtual UserDao Owner { get; set; }
        public virtual GroupDao GroupOwner { get; set; }
        public virtual VerificationKeyDao VerificationKey { get; set; }
        public virtual ISet<MailDao> RecipeMails { get; protected set; }
        public virtual ISet<MailDao> Mails { get; protected set; }


        /// <exception cref="ArgumentNullException"><paramref name="user" /> is <see langword="null" />.</exception>
        public EmailAddressDao(UserDao user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Emails.Add(this);
            Owner = user;
        }

        public EmailAddressDao(GroupDao group, string email)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            if (group == null)
            {
                throw new ArgumentNullException("email");
            }
            Address = email;
            group.GroupAddress = this;           
            GroupOwner = group;
        }
        public virtual void AddMail(MailDao mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException( "mail" );
            }

            if (!RecipeMails.Contains( mail ))
            {
                RecipeMails.Add( mail );
                mail.AddRecipient( this );
            }
        }

        public virtual void AddVerificationKey(VerificationKeyDao key)
        {
            if (key == null)
            {
                throw new ArgumentNullException( "key" );
            }
            key.EmailAddress = this;
            VerificationKey = key;
        }

        public static EmailAddressDao GetEmailAddressById(ISession session, int emailAddressId)
        {
            if (session == null)
            {
                throw new ArgumentNullException( "session" );
            }
            return session.Get<EmailAddressDao>( emailAddressId );
        }

        public static bool IsEmailAddressExists(ISession session, string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            return session.Query<EmailAddressDao>().SingleOrDefault( x => x.Address == email ) != null;
        }

        public static EmailAddressDao GetDaoObject(ISession session, string email)
        {
            if (session == null)
            {
                throw new ArgumentNullException( "session" );
            }
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            return session.Query<EmailAddressDao>().FirstOrDefault( x => x.Address == email );
        }

        public virtual MailEntity SaveRecipient(MailEntity mail, IEnumerable<string> emailAddresses)
        {
            if (mail == null)
            {
                throw new ArgumentNullException( "mail" );
            }
            if (emailAddresses == null)
            {
                throw new ArgumentNullException( "emailAddresses" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var mailDao = MailDao.FindById( uow.Session, mail.Id );                
                foreach (var address in emailAddresses)
                {
                    var emailAddress = GetDaoObject( uow.Session, address );
                    if (emailAddress == null)
                    {
                        emailAddress = new EmailAddressDao { Address = address };
                    }

                    mailDao.Recipients.Add( emailAddress );
                    emailAddress.RecipeMails.Add( mailDao );
                }
                uow.Session.SaveOrUpdate( mailDao );
                uow.TransactionalFlush();
                return MailEntity.ToBusinessItem( mailDao );
            }
        }
        public virtual EmailAddressEntity GetEmailAddressByName(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var emailDao = uow.Session.Query<EmailAddressDao>().SingleOrDefault( x => x.Address == email );
                return EmailAddressEntity.ToBussinessItem( emailDao );
            }
        }

        public virtual bool IsEmailAddressExists(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                return uow.Session.Query<EmailAddressDao>().SingleOrDefault( x => x.Address == email ) != null;
            }
        }

        public virtual EmailAddressEntity AddNewEmail(string email, UserEntity ownerEntity)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            if (ownerEntity == null)
            {
                throw new ArgumentNullException( "ownerEntity" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var existingEmailAddress = IsEmailAddressExists( uow.Session, email );
                if (existingEmailAddress == false)
                {
                    var ownerDao = UserDao.GetUserByUsername(uow.Session, ownerEntity.Login);
                    var emailaddress = new EmailAddressDao(ownerDao) { Address = email };
                    uow.Session.Save(emailaddress);
                    uow.TransactionalFlush();
                    return EmailAddressEntity.ToBussinessItem(emailaddress);
                }             
                return null;
            }
        }

        public virtual EmailAddressEntity AddNewEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var existingEmailAddress = IsEmailAddressExists( uow.Session, email );
                if (existingEmailAddress == false)
                {
                    var emailaddress = new EmailAddressDao { Address = email };
                    uow.Session.Save( emailaddress );
                    uow.TransactionalFlush();
                    return EmailAddressEntity.ToBussinessItem( emailaddress );
                }
                return null;
            }
        }

        public virtual EmailAddressEntity AddNewEmail(string email, GroupEntity ownerEntity)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            if (ownerEntity == null)
            {
                throw new ArgumentNullException( "ownerEntity" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var existingEmailAddress = IsEmailAddressExists( uow.Session, email );
                if (existingEmailAddress == false)
                {
                    var ownerGroup = new GroupDao();
                    ownerGroup = ownerGroup.FindGroupByName( uow.Session, ownerEntity.GroupName );
                    var emailAddress = new EmailAddressDao( ownerGroup, email );
                    uow.Session.Save( emailAddress );
                    uow.TransactionalFlush();
                    return EmailAddressEntity.ToBussinessItem( emailAddress );
                }
                return null;
            }
        }
    }


    [Serializable]
    public class EmailAlreadyHasOwnerExeption : Exception
    {
        public EmailAlreadyHasOwnerExeption() { }
        public EmailAlreadyHasOwnerExeption(string message) : base(message) { }
        public EmailAlreadyHasOwnerExeption(string message, Exception inner) : base(message, inner) { }
        protected EmailAlreadyHasOwnerExeption(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}