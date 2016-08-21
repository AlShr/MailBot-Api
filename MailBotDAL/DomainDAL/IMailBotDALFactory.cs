namespace MailBot.DataAccessLayer.DomainDAL
{
    public interface IMailBotDALFactory
    {
        IUserDAL CreateUserDAL();
        IMailDAL CreateMailDAL();
        IEmailAddressDAL CreateEmailAddressDAL();
        IVerificationKeyDAL CreateVKeyDAL();
        IGroupDAL CreateGroupDAL();
        IMessageIdDAL CreateMessageIdDAL();
    }
}