using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public interface IVerificationKeyDAL
    {
        void ValidateEmailAddress(string email);
        string GetVerificationCodeByEmail(string email);
        VerificationKeyEntity CreateVerificationEmailRecords(string email, string code);
    }
}