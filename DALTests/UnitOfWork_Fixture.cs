using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Reflection;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;


namespace DALTests
{
   
    [TestFixture]
    public class UnitOfWorkFixtureWithFactoryFexture
    {
        private readonly MockRepository mocks = new MockRepository();
        private IUnitOfWorkFactory factory;
        private IUnitOfWork unitOfWork;
        private ISession session;
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ResetUnitOfWork();
        }
        [Test]
        public void CanStartUnitOfWork()
        {
            factory = mocks.DynamicMock<IUnitOfWorkFactory>();
            unitOfWork = mocks.DynamicMock<IUnitOfWork>();
            session = mocks.DynamicMock<ISession>();
            InstrumentUnitOfWork();
            mocks.BackToRecordAll();
            SetupResult.For(factory.Create()).Return(unitOfWork);          
            mocks.ReplayAll();
        }
        [TearDown]
        public void TearDownContext()
        { 
            ResetUnitOfWork();
        }
        private void InstrumentUnitOfWork()
        {
            // brute force attack to set my own factory via reflection
            var fieldInfo = typeof(UnitOfWork).GetField("unitOfWorkFactory",
                BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            fieldInfo.SetValue(null, factory);
        }
        private void ResetUnitOfWork()
        {
            // assert that the UnitOfWork is reset
            var fieldInfo = typeof(UnitOfWork).GetField("innerUnitOfWork",
                         BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
            fieldInfo.SetValue(null, null);
        }
   
        [Test]
        public void AccessingCurrentUnitOfWorkIfNotStartedThrows()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var current = UnitOfWork.Current;
            });
        }
        [Test]
        public void CanTestUnitOfWorkIsStarted()
        {
            Assert.IsFalse(UnitOfWork.IsStarted);
            IUnitOfWork uow = UnitOfWork.Start();
            Assert.IsTrue(UnitOfWork.IsStarted);
        }
        [Test]
        public void CanAccessCurentUnitOfWork()
        {
            IUnitOfWork uow = UnitOfWork.Start();
            var current = UnitOfWork.Current;
            Assert.AreSame(uow, current);
        }
      
    }
}
