using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public interface IMessageIdDAL
    {
        void SaveMessageId(MailEntity mail, string hash);
        void SaveMessageId(MailEntity mail, string[] hashes);
        void SetParentMail(int mailId);
    }
}
