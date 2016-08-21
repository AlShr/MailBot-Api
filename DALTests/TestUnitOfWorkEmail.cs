using DALTests.Generators;
using NUnit.Framework;
using System;
using System.Reflection;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using NHibernate.Linq;
using System.Linq;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAcessLayer.DomainDAL;
using System.Collections.Generic;

namespace DALTests
{
    [TestFixture]
    public class TestUnitOfWorkEmail
    {
        IUserDAL userDAL;
        IEmailAddressDAL emailDAL;
        IMailBotDALFactory dALFactory;
        [SetUp]
        public void SetupContext()
        {
            UnitOfWork.Configuration.AddAssembly(Assembly.GetExecutingAssembly());
            //just for create db in  first  
           // new SchemaExport(UnitOfWork.Configuration).Execute(false, true, false); 
            dALFactory = new MailBotDALFactory();
            userDAL = dALFactory.CreateUserDAL();
            emailDAL = dALFactory.CreateEmailAddressDAL();
        }

        [Test]
        public void CanAddNewEmailAddressForUser()
        {
            
            UserEntity user = new UserEntity(){  Login=Generator.LettersNumbers(6).ToString(),Password=Generator.LettersNumbers(6).ToString() };
            var currentUser = userDAL.RegisterNewUser(user);
            string emailData = Generator.GenerateEmailAddress().ToString();
            var userEmail = emailDAL.AddNewEmail(emailData, currentUser);
            Assert.IsNotNull(userEmail);
           
        }
        [Test]
        public void CanGetUserMailboxes()
        {
            UserEntity user = new UserEntity(){ Login="alena123", Password="alena_123" };
            var userlogged = userDAL.Autentication(user);
            List<EmailAddressEntity> emailboxes = new List<EmailAddressEntity>();
            emailboxes = userDAL.GetUserMailboxes(userlogged);
            Assert.IsTrue(emailboxes.Count != 0);

        }
        [Test]
        public void CanGetUserMailbyEmails()
        {
            UserEntity user = new UserEntity() { Login = "alena123", Password = "alena_123" };
            var mailuser = userDAL.GetAllUserMessages(user);
        }
        
    }
}
