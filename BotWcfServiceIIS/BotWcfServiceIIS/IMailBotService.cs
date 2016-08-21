using System.Collections.Generic;
using System.ServiceModel;
using MailBot.Service.MailBotServiceDTO;

namespace MailBot.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBotMailService" in both code and config file together.
    [ServiceContract]
    public interface IMailBotService
    {
        [OperationContract, FaultContract(typeof(TrackedFault))]
        UserProxy AuthenticateUser(UserProxy credentials);

        [OperationContract]
        UserProxy RegisterUser(UserProxy credentials);

        [OperationContract]
        EmailAddressProxy AddEmailAddress(UserProxy credentials, string email);

        [OperationContract]
        EmailAddressProxy AddGroupEmailAddress(GroupProxy owner, string email);

        [OperationContract]
        VerificationKeyProxy VerifyEmailAddress(UserProxy userInfo, string email);

        [OperationContract]
        VerificationKeyProxy VerifyGroupEmailAddress(GroupProxy groupInfo, string email);

        [OperationContract]
        bool ConfirmVerificationCode(string email, string code);

        [OperationContract]
        List<MailMessageProxy> GetAllMessagesForUser(UserProxy userInfo);

        [OperationContract]
        List<EmailAddressProxy> GetUserMailboxes(UserProxy proxy);

        [OperationContract]
        GroupProxy RegisterGroup(string groupName);

        [OperationContract]
        bool IsServiceConfigured();

        [OperationContract]
        List<MailMessageProxy> SearchInMessages(string searchQuery, string[] fields);

        [OperationContract]
        UserProxy AddUserToGroup(UserProxy credentials, GroupProxy groupInfo);
    }
}