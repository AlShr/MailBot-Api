using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Data;
using MailBot.DataAccessLayer.UnitOfWorkHelpers;


namespace DALTests
{
    [TestFixture]
    public class UnitOfWorkImplementorFixture
    {
        private readonly MockRepository mocks = new MockRepository();
        private IUnitOfWorkFactory factory;
        private ISession session;
        private IUnitOfWorkImplementor uow;
        [SetUp]
        public void SetupContext()
        {
            factory = mocks.DynamicMock<IUnitOfWorkFactory>();
            session = mocks.DynamicMock<ISession>();
        }
        [Test]
        public void CanDisposeUnitOfWorkImplementor()
        {
            using (mocks.Record())
            {
                Expect.Call(() => factory.DisposeUnitOfWork(null)).IgnoreArguments();
            }
            using (mocks.Playback())
            {
                uow = new UnitOfWorkImplementor(factory);
                uow.Dispose();
            }
        }
        [Test]
        public void CanBeginTransaction()
        {
            using (mocks.Record())
            {
                Expect.Call(session.BeginTransaction()).Return(null);
            }
            using (mocks.Playback())
            {
                uow = new UnitOfWorkImplementor(factory);
                (uow as UnitOfWorkImplementor).Session = session;
                var transaction = uow.BeginTransaction();
                Assert.IsNotNull(transaction);
            }
        }

        [Test]
        public void Can_BeginTransaction_specifying_isolation_level()
        {
            //if the system detects a write collision among several concurrent 
            //transactions, only one of them is allowed to commit
            var isolationLevel = IsolationLevel.Serializable;
            using (mocks.Record())
            {
                Expect.Call(session.BeginTransaction(isolationLevel)).Return(null);
            }
            using (mocks.Playback())
            {
                uow = new UnitOfWorkImplementor(factory);
                (uow as UnitOfWorkImplementor).Session = session;
                var transaction = uow.BeginTransaction(isolationLevel);
                Assert.IsNotNull(transaction);
            }
        }

        //The TransactionalFlush method should flush the content of the NHibernate session to the database
        //wrapped inside a transaction
        [Test]
        public void Can_execute_TransactionalFlush()
        {
            var tx = mocks.CreateMock<ITransaction>();
            var session = mocks.DynamicMock<ISession>();
            SetupResult.For(session.BeginTransaction(IsolationLevel.ReadCommitted)).Return(tx);
            uow = mocks.PartialMock<UnitOfWorkImplementor>(factory);
            (uow as UnitOfWorkImplementor).Session = session;
            using (mocks.Record())
            {
                Expect.Call(tx.Commit);
                Expect.Call(tx.Dispose);
            }
            using (mocks.Playback())
            {
                uow = new UnitOfWorkImplementor(factory);
                (uow as UnitOfWorkImplementor).Session = session;
                uow.TransactionalFlush();

            }
        }
    }
}
