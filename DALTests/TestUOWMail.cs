using DALTests.Generators;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using MailBot.DataAcessLayer.DomainDAL;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Text;


namespace DALTests
{
    [TestFixture]
    public class TestUOWMail
    {
        IMailBotDALFactory dal;
        IEmailAddressDAL emailDal;
        IMessageIdDAL messageIdDal;
        IMailDAL mailDal;
        [SetUp]
        public void SetupContext()
        {
            UnitOfWork.Configuration.AddAssembly(Assembly.GetExecutingAssembly());
            dal = new MailBotDALFactory();
            emailDal = dal.CreateEmailAddressDAL();
            messageIdDal = dal.CreateMessageIdDAL();
            mailDal = dal.CreateMailDAL();
        }
        [Test]
        public void CanSaveСonversation()
        {
            string email = "test2@mail.local";
            var emailEntity = emailDal.IsEmailAddressExists(email) == true ?
                emailDal.GetEmailAddressByName(email) : emailDal.AddNewEmail(email);
            Assert.IsNotNull(emailEntity);
            var mailEntity = mailDal.AddMail("Hello", "Hello to you", emailEntity);
            Assert.IsNotNull(mailEntity);            
            messageIdDal.SaveMessageId(mailEntity, "e4ts81");
            string[] hashes = { "d57pjn", Generator.LettersNumbers(8).ToString() };
            messageIdDal.SaveMessageId(mailEntity, hashes);
            messageIdDal.SetParentMail(mailEntity.Id);
           
        }
        [Test]
        public void CanSaveMail()
        {
            string email = "test2@mail.local";
            var emailEntity = emailDal.IsEmailAddressExists(email) == true ?
                emailDal.GetEmailAddressByName(email) : emailDal.AddNewEmail(email);
            Assert.IsNotNull(emailEntity);
            var mailEntity = mailDal.AddMail("Hello", "Hello", emailEntity);
            Assert.IsNotNull(mailEntity);
            messageIdDal.SaveMessageId(mailEntity, "d57pjn");
        }
    }
}
