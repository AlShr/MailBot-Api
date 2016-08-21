using System;
using System.Collections.Generic;
using System.Linq;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using MailBot.Security;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Search.Attributes;

namespace MailBot.DataAccessLayer.DomainDAL
{
    [Indexed]
    public class UserDao : IUserDAL
    {
        public UserDao()
        {
            Groups = new HashSet<GroupDao>();
            Emails = new HashSet<EmailAddressDao>();
        }

        public virtual int Id { get; protected set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual ISet<GroupDao> Groups { get; protected set; }
        public virtual ISet<EmailAddressDao> Emails { get; protected set; }

        internal static List<MailDao> GetAllMessagesbyUserAddress(ISession session, UserDao userDao)
        {
            if (userDao == null)
            {
                throw new ArgumentNullException( "userEntity" );
            }
            var emails = userDao.Emails.ToList();
            var mails = new List<MailDao>();
            emails.ForEach( x => mails.AddRange( x.RecipeMails ) );
            return mails;
        }

        internal static List<MailDao> GetAllMessagesbyUsersGroups(ISession session, UserDao userDao)
        {
            if (userDao == null)
            {
                throw new ArgumentNullException( "userEntity" );
            }
            var groups = userDao.Groups.ToList();
            var groupEmails = new List<EmailAddressDao>();
            groups.ForEach( x => groupEmails.Add( x.GroupAddress ) );
            var mails = new List<MailDao>();
            //здесь не работает!
            groupEmails.ForEach( x => mails.AddRange( x.RecipeMails ) );
            return mails;
        }

        public virtual UserEntity AddUserToGroup(GroupEntity groupEntity, UserEntity userEntity)
        {
            if (groupEntity == null)
            {
                throw new ArgumentNullException( "groupEntity" );
            }
            if (userEntity == null)
            {
                throw new ArgumentNullException( "userEntity" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var groupDao = new GroupDao();
                groupDao = groupDao.FindGroupByName( uow.Session, groupEntity.GroupName );
                var user = GetUserByUsername( uow.Session, userEntity.Login );
                groupDao.AddUser( user );
                uow.Session.Merge( groupDao );
                uow.TransactionalFlush();
                uow.Session.Close();
                return userEntity;
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="login" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="password" /> is <see langword="null" />.</exception>
        public virtual UserEntity RegisterNewUser(UserEntity userEntity)
        {
            if (userEntity == null)
            {
                throw new ArgumentNullException( "userEntity" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var existingUser = GetUserByUsername( uow.Session, userEntity.Login );
                if (existingUser == null)
                {
                    var user = new UserDao { Login = userEntity.Login, Password = Hasher.Encrypt( userEntity.Password ) };
                    uow.Session.Save( user );
                    uow.TransactionalFlush();
                    return UserEntity.ToBusinessItem( user );
                }
                return null;
            }
        }

        public virtual UserEntity Autentication(UserEntity userentity)
        {
            if (userentity == null)
            {
                throw new ArgumentNullException( "userentity" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var userDao = uow.Session.Query<UserDao>().FirstOrDefault( x => x.Login == userentity.Login );
                if (userDao == null)
                {
                    return null;
                }
                return userDao.Password == Hasher.Encrypt( userentity.Password ) || userDao.Password == userentity.Password
                    ? UserEntity.ToBusinessItem( userDao )
                    : null;
            }
        }

        #region Mail functionality 

        public virtual List<EmailAddressEntity> GetUserMailboxes(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException( "user" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var emails = uow.Session
                    .Query<VerificationKeyDao>()
                    .Where( x => x.EmailAddress.Owner.Login == user.Login )
                    .Select( x => x.EmailAddress )
                    .ToList();
                var emailEntities = new List<EmailAddressEntity>();
                emails.ForEach( x => emailEntities.Add( EmailAddressEntity.ToBussinessItem( x ) ) );
                return emailEntities;
            }
        }

        #endregion

        public virtual List<MailEntity> GetAllUserMessages(UserEntity userEntity)
        {
            if (userEntity == null)
            {
                throw new ArgumentNullException( "userEntity" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var user = GetUserByUsername( uow.Session, userEntity.Login );
                var mails = GetAllMessagesbyUserAddress( uow.Session, user );
                var groupMails = GetAllMessagesbyUsersGroups( uow.Session, user );
                mails.AddRange( groupMails );
                var resultList = new List<MailEntity>();
                foreach (var mail in mails)
                {
                    resultList.Add( MailEntity.ToBusinessItem( mail ) );
                }
                return resultList;
            }
        }

        #region Group functionality

        /// <exception cref="ArgumentNullException"><paramref name="group" /> is <see langword="null" />.</exception>
        public virtual void AddGroup(GroupDao group)
        {
            if (group == null)
            {
                throw new ArgumentNullException( "group" );
            }
            if (!Groups.Contains( group ))
            {
                Groups.Add( group );
                group.AddUser( this );
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="group" /> is <see langword="null" />.</exception>
        public virtual void RemoveFrom(GroupDao group)
        {
            if (group == null)
            {
                throw new ArgumentNullException( "group" );
            }
            if (Groups.Contains( group ))
            {
                Groups.Remove( group );
                group.RemoveUser( this );
            }
        }

        #endregion

        #region Methods for user searching

        /// <exception cref="ArgumentNullException"><paramref name="username" /> is <see langword="null" />.</exception>
        public static UserDao GetUserByUsername(ISession session, string username)
        {
            if (session == null)
            {
                throw new ArgumentNullException( "session" );
            }
            if (username == null)
            {
                throw new ArgumentNullException( "username" );
            }
            return session
                .Query<UserDao>()
                .FirstOrDefault( x => x.Login == username );
        }

        //call only when you havent opened session
        public virtual UserDao GetUserByUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                return uow.Session
                .Query<UserDao>()
                .FirstOrDefault(x => x.Login == username);
            }
        }

        public virtual UserEntity GetUserByUsername(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException( "user" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                return UserEntity.ToBusinessItem( uow.Session
                    .Query<UserDao>()
                    .FirstOrDefault( x => x.Login == user.Login ) );
            }
        }

        /// <exception cref="ArgumentNullException"><paramref name="emailAddress" /> is <see langword="null" />.</exception>
        public virtual UserEntity GetUserByEmailAddress(string emailAddress)
        {
            if (emailAddress == null)
            {
                throw new ArgumentNullException( "emailAddress" );
            }
            using (var uow = UnitOfWork.Start() as UnitOfWorkImplementor)
            {
                var email = uow.Session.Query<EmailAddressDao>().FirstOrDefault( x => x.Address == emailAddress );
                return email != null
                    ? UserEntity.ToBusinessItem( email.Owner )
                    : null;
            }
        }

        #endregion
    }
}