using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using NHibernate;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public interface IMailDAL
    {
        MailEntity AddMail(string subject, string body, EmailAddressEntity sender);
        MailEntity FindById(int id);
        MailEntity SetMailMessageId(MailEntity mail, string[] hashes);       
    }
}