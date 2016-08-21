using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.BusinessLayer.BusinessLogic;
using NUnit.Framework;
using Rhino.Mocks;
using MailBot.DataAccessLayer.DomainDAL;

namespace BLTests
{
    [TestFixture]
    public class TestUserBCFixture
    {
        private readonly MockRepository mocks = new MockRepository();
        [Test]
        public void TestUserBCAutenticateUser()
        {
            var mockUser = new NMock.DynamicMock(typeof(IUserEntity));
            var mockUserDao = new NMock.DynamicMock(typeof(IUserDAL));
            mockUser.SetupResult("Login", "alena123", typeof(string));
            mockUser.SetupResult("Password", "alena_123", typeof(string));
            IUserEntity user = (IUserEntity)mockUser.MockInstance;
            IUserDAL userDao = (IUserDAL)mockUserDao.MockInstance;
            mockUser.Expect("Validate");
            mockUserDao.Expect("Autentication", user);
            UserEntity userBll = new UserEntity(userDao);
            userBll.AutenticateUser(user);
            mockUser.Verify();
        }
    }
}
