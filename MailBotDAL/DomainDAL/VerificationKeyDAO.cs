using System;
using System.Linq;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using NHibernate;
using NHibernate.Linq;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public class VerificationKeyDao : IVerificationKeyDAL
    {
        public virtual int Id { get; protected set; }
        public virtual string Key { get; set; }
        public virtual string Status { get; set; }
        public virtual EmailAddressDao EmailAddress { get; set; }

        public virtual void ValidateEmailAddress(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var vKey = GetDaoObjectByEmail( uow.Session, email );
                vKey.Status = "true";
                uow.Session.SaveOrUpdate( vKey );
                uow.TransactionalFlush();
            }
        }

        #region Methods for searching 

        /// <exception cref="ArgumentNullException"><paramref name="email" /> is <see langword="null" />.</exception>
        public virtual string GetVerificationCodeByEmail(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var emailRecord = uow.Session.Query<EmailAddressDao>().FirstOrDefault( x => x.Address == email );
                return ( emailRecord != null )
                    ? emailRecord.VerificationKey.Key
                    : null;
            }
        }

        public virtual VerificationKeyEntity CreateVerificationEmailRecords(string email, string code)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            if (code == null)
            {
                throw new ArgumentNullException( "code" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var emailDao = EmailAddressDao.GetDaoObject( uow.Session, email );
                var vkeyDao = new VerificationKeyDao { Key = code, EmailAddress = emailDao, Status = "false" };
                emailDao.VerificationKey = vkeyDao;
                uow.Session.Save( vkeyDao );
                uow.Session.SaveOrUpdate( emailDao );
                uow.TransactionalFlush();
                return VerificationKeyEntity.ToBussinessItem( vkeyDao );
            }
        }

        public virtual VerificationKeyDao GetDaoObjectByEmail(ISession session, string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException( "email" );
            }
            return session.
                Query<VerificationKeyDao>().
                FirstOrDefault( x => x.EmailAddress.Address == email );
        }

        public static VerificationKeyDao GetVerificationKeyById(ISession session, int id)
        {
            if (session == null)
            {
                throw new ArgumentNullException( "session" );
            }
            var vKey = session.Get<VerificationKeyDao>( id );
            return vKey;
        }

        public static VerificationKeyDao GetDaoByCode(ISession session, string code)
        {
            if (session == null)
            {
                throw new ArgumentNullException( "session" );
            }
            if (code == null)
            {
                throw new ArgumentNullException( "code" );
            }
            return session.
                Query<VerificationKeyDao>().
                FirstOrDefault( x => x.Key == code );
        }

        #endregion
    }
    [Serializable]
    public class NotVerificatedAdressException : Exception
    {
        public NotVerificatedAdressException() { }
        public NotVerificatedAdressException(string message) : base(message) { }
        public NotVerificatedAdressException(string message, Exception inner) : base(message, inner) { }
        protected NotVerificatedAdressException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}