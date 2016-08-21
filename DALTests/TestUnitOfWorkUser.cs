using System;
using System.Reflection;
using NUnit.Framework;
using System.Text;
using Rhino.Mocks;
using DALTests.Generators;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.BusinessLayer;
using MailBot.DataAcessLayer.DomainDAL;


namespace DALTests
{
    //our implementation thread safe

    [TestFixture]
    public class TestUnitOfWorkUser
    {       
        [SetUp]
        public void SetupContext()
        {
            UnitOfWork.Configuration.AddAssembly(Assembly.GetExecutingAssembly());
            //just for create db in  first  
            // new SchemaExport(UnitOfWork.Configuration).Execute(false, true, false); 

        }
        //[Test]
        //public void CanUseRegisterNewUser()
        //{
        //    MailBotDALFactory dalFactory = new MailBotDALFactory();
        //    IUserDAL userDAL = dalFactory.CreateUserDAL();
        //    UserEntity userbl = new UserEntity() { Login = Generator.LettersNumbers(6).ToString(), Password = Generator.LettersNumbers(6).ToString() };
        //    userDAL.RegisterNewUser(userbl);
        //    UserDao userDao = new UserDao();
        //    var currentUser = userDAL.RegisterNewUser(userbl);
        //    Assert.IsNotNull(currentUser);

        //}
        //[Test]
        //public void CanAutenticateUser()
        //{
        //    MailBotDALFactory dalFactory = new MailBotDALFactory();
        //    IUserDAL userDAL = dalFactory.CreateUserDAL();
        //    UserEntity userbl = new UserEntity() { Login = Generator.LettersNumbers(6).ToString(), Password = Generator.LettersNumbers(6).ToString() };
        //    var currentUser = userDAL.RegisterNewUser(userbl);
        //    Assert.IsNotNull(currentUser);
        //    var autenticateUser = userDAL.Autentication(currentUser);
        //    Assert.IsNotNull(autenticateUser);
        //}
    }
}
