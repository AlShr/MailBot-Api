using System;
using System.Collections.Generic;
using System.ServiceModel;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using MailBot.DataAcessLayer.DomainDAL;
using MailBot.SearchIndexing;
using MailBot.Service.Config;
using MailBot.Service.Converter;
using MailBot.Service.ErrorHandling;
using MailBot.Service.MailBotServiceDTO;
using MailBot.Service.MailFetching;

namespace MailBot.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MailBotService : IMailBotService, IDisposable
    {
        private readonly IMailBotDALFactory dalFactory;

        public MailBotService()
        {
            ConfigureService();
            BotMailbox.GetImapInstance().OnNewMessage += MailSaving.ReactToNewMessage;
            dalFactory = new MailBotDALFactory();
        }

        private static void ConfigureService()
        {
            MailboxInfoProxy box;
            HostInfoProxy imapInfo;
            HostInfoProxy smtpInfo;
            ConfigFetcher.FetchConfiguration( out box, out imapInfo, out smtpInfo );
            ConfigFetcher.InitializeMailClients( box, imapInfo, smtpInfo );
        }

        public void Dispose()
        {
            UnitOfWork.Current.Dispose();
        }

        public UserProxy AuthenticateUser(UserProxy credentials)
        {
            try
            {
                var userEntity = PolymorphConverter.ToBusinessItem( credentials );
                var authenticationResult = dalFactory.CreateUserDAL().Autentication( userEntity );
                return PolymorphConverter.ToProxyItem( authenticationResult );
            }
            catch (ArgumentNullException ex)
            {
                throw FaultTracker.ReturnTrackedFault( ex, "Authentication." );
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault( e, "500." );
            }
        }

        /// <summary>
        /// Add user to DB. (without email addresses).
        /// </summary>
        /// <param name="credentials">Pair login-pass</param>
        /// <returns></returns>
        public UserProxy RegisterUser(UserProxy credentials)
        {
            try
            {
                var userEntity = PolymorphConverter.ToBusinessItem( credentials );
                var registerResult = dalFactory.CreateUserDAL().RegisterNewUser( userEntity );
                return PolymorphConverter.ToProxyItem( registerResult );
            }
            catch (ArgumentNullException ex)
            {
                throw FaultTracker.ReturnTrackedFault( ex, "Register." );
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault( e, "500." );
            }
        }

        /// <summary>
        /// Add email address for user (not verified).
        /// </summary>
        /// <param name="credentials">Pair login-pass</param>
        /// <param name="email">New email address</param>
        /// <returns></returns>
        public EmailAddressProxy AddEmailAddress(UserProxy credentials, string email)
        {
            try
            {
                if (email == null)
                {
                    throw new ArgumentException( "there is no such email address" );
                }
                var userEntity = PolymorphConverter.ToBusinessItem( credentials );
                var authenticatedEntity = dalFactory.CreateUserDAL().Autentication( userEntity );

                var emailEntity = dalFactory.CreateEmailAddressDAL().AddNewEmail( email, authenticatedEntity );
                var emailProxy = PolymorphConverter.ToProxyItem( emailEntity );
                return emailProxy;
            }
            catch (ArgumentException ex)
            {
                throw FaultTracker.ReturnTrackedFault( ex, "AddEmailAddress." );
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault( e, "500." );
            }
        }

        /// <summary>
        /// Add email address for group (not verified).
        /// </summary>
        /// <param name="groupInfo">Group name</param>
        /// <param name="email">New email address</param>
        /// <returns></returns>
        public EmailAddressProxy AddGroupEmailAddress(GroupProxy groupInfo, string email)
        {
            try
            {
                if (email == null)
                {
                    throw new ArgumentException("there is no such email address");
                }
                var groupEntity = PolymorphConverter.ToBusinessItem(groupInfo);
                var emailEntity = dalFactory.CreateEmailAddressDAL().AddNewEmail(email, groupEntity);
                var emailProxy = PolymorphConverter.ToProxyItem(emailEntity);
                return emailProxy;

            }
            catch (ArgumentException ex)
            {
                throw FaultTracker.ReturnTrackedFault(ex, "AddEmailAddressGroup");
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault(e, "500.");
            }
        }

        /// <summary>
        /// First step of verification for new email address. Call this method only if email already exist in db (AddEmailAdress).
        /// </summary>
        /// <param name="credentials">Pair login-pass</param>
        /// <param name="email">Not verified email adress</param>
        /// <returns></returns>
        public VerificationKeyProxy VerifyEmailAddress(UserProxy credentials, string email)
        {
            try
            {
                var businessCredentials = PolymorphConverter.ToBusinessItem( credentials );
                var userEntity = dalFactory.CreateUserDAL().Autentication( businessCredentials );
                if (userEntity == null)
                {
                    throw new ArgumentException( "There is no such user with this login-pass pair." );
                }
                var verificationEmail = VerificationRequestTool
                    .InitiateEmailAddressVerification( email, userEntity.Login, userEntity.Password );
                return PolymorphConverter.ToProxyItem( verificationEmail );
            }
            catch (ArgumentException ex)
            {
                throw FaultTracker.ReturnTrackedFault( ex, "VerifyEmailAddress." );
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault( e, "500." );
            }
        }

        /// <summary>
        /// First step of verification for new email address. Call this method only if email already exist in db (AddGroupEmailAdress) .
        /// </summary>
        /// <param name="groupInfo">Group name</param>
        /// <param name="email">Not verified email adress</param>
        /// <returns></returns>
        public VerificationKeyProxy VerifyGroupEmailAddress(GroupProxy groupInfo, string email)
        {
            try
            {
                var businesEntity = PolymorphConverter.ToBusinessItem(groupInfo);
                
                if (businesEntity == null)
                {
                    throw new ArgumentException("There is no such user with this login-pass pair.");
                }
                var verificationEmail = VerificationRequestTool
                    .InitiateEmailAddressVerification(email, groupInfo.GroupName);
                return PolymorphConverter.ToProxyItem(verificationEmail);
            }
            catch (ArgumentException ex)
            {
                throw FaultTracker.ReturnTrackedFault(ex, "VerifyEmailAddress.");
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault(e, "500.");
            }
        }

        /// <summary>
        /// Second step of verification for new email address. Сompares the code in the db and the one that came on mail.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="code">Code that come on mail</param>
        /// <returns></returns>
        public bool ConfirmVerificationCode(string email, string code)
        {
            try
            {
                return VerificationRequestTool.ValidateEmailStatus( email, code );
            }
            catch (ArgumentException ex)
            {
                throw FaultTracker.ReturnTrackedFault( ex, "ConfirmVerificationCode." );
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault( e, "500." );
            }
        }

        public List<EmailAddressProxy> GetUserMailboxes(UserProxy credentials)
        {
            var emails = dalFactory.CreateUserDAL().GetUserMailboxes( PolymorphConverter.ToBusinessItem( credentials ) );
            var emailsproxy = new List<EmailAddressProxy>();
            emails.ForEach( x => emailsproxy.Add( PolymorphConverter.ToProxyItem( x ) ) );
            return emailsproxy;
        }

        public List<MailMessageProxy> GetAllMessagesForUser(UserProxy credentials)
        {
            var userEntity = dalFactory.CreateUserDAL().Autentication( PolymorphConverter.ToBusinessItem( credentials ) );
            if (userEntity == null)
            {
                throw new ArgumentException( "there is no such user with this login-pass pair" );
            }
            return dalFactory.CreateUserDAL().GetAllUserMessages( userEntity ).ConvertAll( PolymorphConverter.ToProxyItem );
        }

        /// <summary>
        /// Add new group in db
        /// </summary>
        /// <param name="credentials">Unique name for group</param>
        /// <returns></returns>
        public GroupProxy RegisterGroup(string groupName)
        {
            try
            {
                return PolymorphConverter.ToProxyItem( dalFactory.CreateGroupDAL().RegisterNewGroup( groupName ) );
            }
            catch (ArgumentException ex)
            {
                throw FaultTracker.ReturnTrackedFault( ex, "AddGroup." );
            }
            catch (Exception e)
            {
                throw FaultTracker.ReturnTrackedFault( e, "500." );
            }
        }

        public bool IsServiceConfigured()
        {
            return false;
        }

        public List<MailMessageProxy> SearchInMessages(string searchQuery, string[] fields)
        {
            var findedMessagesId = LuceneIndex.Instance.Search( searchQuery, fields );
            var result = new List<MailMessageProxy>();
            foreach (var messageId in findedMessagesId)
            {
                var message = dalFactory.CreateMailDAL().FindById(messageId);
                var converted = PolymorphConverter.ToProxyItem(message);
                result.Add(converted);
            }
            return result;
            //return findedMessagesId.Select(messageId => PolymorphConverter.ToProxyItem(MailBC.FindMailById(messageId))).ToList();
        }

        public UserProxy AddUserToGroup(UserProxy credentials, GroupProxy groupInfo)
        {
            //not tested
            try
            {
                var userEntity = PolymorphConverter.ToBusinessItem(credentials);
                var groupEntity = PolymorphConverter.ToBusinessItem(groupInfo);
                userEntity = dalFactory.CreateUserDAL().AddUserToGroup(groupEntity, userEntity);
                return PolymorphConverter.ToProxyItem(userEntity);
            }
            catch(Exception e)
            {
                throw FaultTracker.ReturnTrackedFault(e, "some exeption");
            }
        }
    }
}