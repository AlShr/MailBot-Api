using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Reflection;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;

namespace DALTests
{
    [TestFixture]
    public class UnitOfWorkFactoryFixture
    {
        private IUnitOfWorkFactory factory;
        [SetUp]
        public void SetupContext()
        {
            factory = (IUnitOfWorkFactory)Activator.CreateInstance(typeof(UnitOfWorkFactory), true);
        }
        [Test]
        public void CanCreateUnitOfWork()
        {
            IUnitOfWork implementor = factory.Create();
            Assert.IsNotNull(implementor);
            Assert.IsNotNull(factory.CurrentSession);
            Assert.AreEqual(FlushMode.Commit, factory.CurrentSession.FlushMode);
        }
        //[Test]
        //public void Can_configure_NHibernate()
        //{
        //    var configuration = factory.Configuration;
        //    Assert.IsNotNull(configuration);
        //}

        //The access to the NHibernate SessionFactory is thread safe!
        [Test]
        public void CanCreateAndAccessSessionFactory()
        {
            var sessionFactory = factory.SessionFactory;
            Assert.IsNotNull(sessionFactory);

        }

    }
}
