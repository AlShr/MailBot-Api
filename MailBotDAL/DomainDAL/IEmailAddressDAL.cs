using System.Collections.Generic;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public interface IEmailAddressDAL
    {
        MailEntity SaveRecipient(MailEntity mail, IEnumerable<string> emailAddresses);
        EmailAddressEntity GetEmailAddressByName(string email);
        bool IsEmailAddressExists(string email);
        EmailAddressEntity AddNewEmail(string email, UserEntity ownerEntity);
        EmailAddressEntity AddNewEmail(string email);
        EmailAddressEntity AddNewEmail(string email, GroupEntity ownerEntity);
    }
}