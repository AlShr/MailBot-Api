using MailBot.DataAccessLayer.DomainDAL;

namespace MailBot.DataAcessLayer.DomainDAL
{
    public class MailBotDALFactory : IMailBotDALFactory
    {
        public IUserDAL CreateUserDAL()
        {
            return new UserDao();
        }

        public IMailDAL CreateMailDAL()
        {
            return new MailDao();
        }

        public IEmailAddressDAL CreateEmailAddressDAL()
        {
            return new EmailAddressDao();
        }

        public IVerificationKeyDAL CreateVKeyDAL()
        {
            return new VerificationKeyDao();
        }

        public IGroupDAL CreateGroupDAL()
        {
            return new GroupDao();
        }
        public IMessageIdDAL CreateMessageIdDAL()
        {
            return new MessageIdDao();
        }
    }
}