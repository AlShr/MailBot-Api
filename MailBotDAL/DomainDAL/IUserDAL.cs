using System.Collections.Generic;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public interface IUserDAL
    {
        UserEntity Autentication(UserEntity userEntity);
        UserEntity RegisterNewUser(UserEntity userentity);
        UserEntity AddUserToGroup(GroupEntity group, UserEntity user);
        UserEntity GetUserByUsername(UserEntity user);
        UserDao GetUserByUsername(string username);
        List<EmailAddressEntity> GetUserMailboxes(UserEntity user);
        UserEntity GetUserByEmailAddress(string emailAddress);
        List<MailEntity> GetAllUserMessages(UserEntity username);
    }
}