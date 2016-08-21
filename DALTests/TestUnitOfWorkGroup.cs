using DALTests.Generators;
using NUnit.Framework;
using System.Reflection;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using MailBot.DataAcessLayer.DomainDAL;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;

namespace DALTests
{
    [TestFixture]
    public class TestUnitOfWorkGroup
    {
        IMailBotDALFactory dal;
        IEmailAddressDAL emailDal;
        IGroupDAL groupDAL;
        [SetUp]
        public void SetupContext()
        {
            UnitOfWork.Configuration.AddAssembly(Assembly.GetExecutingAssembly());
            dal = new MailBotDALFactory();
            emailDal = dal.CreateEmailAddressDAL();
            groupDAL = dal.CreateGroupDAL();
        }
        [Test]
        public void CanUseRegisterNewGroup()
        {
            var group = groupDAL.RegisterNewGroup("admin");
            Assert.IsNotNull(group);
            var email = emailDal.AddNewEmail("test2@mail.local", group);
            group = groupDAL.FindGroupByName("admin");
            Assert.IsNotNull(group);
            Assert.IsNotNull(group.GroupAddress);
        }      
    }
}
