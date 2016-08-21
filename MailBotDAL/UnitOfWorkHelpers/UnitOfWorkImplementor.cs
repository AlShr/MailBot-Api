using System;
using System.Data;
using NHibernate;

namespace MailBot.DataAccessLayer.UnitOfWorkHelpers
{
    //This class defines an actual Unit of Work instance
    public class UnitOfWorkImplementor : IUnitOfWorkImplementor
    {
        private readonly IUnitOfWorkFactory factory;
        private ISession session;

        public UnitOfWorkImplementor(IUnitOfWorkFactory factory)
        {
            this.factory = factory;
        }

        public ISession Session
        {
            set { session = value; }
            get { return session; }
        }

        public bool IsInActiveTransaction
        {
            get { return session.Transaction.IsActive; }
        }

        public IUnitOfWorkFactory Factory
        {
            get { return factory; }
        }

        public void Flush()
        {
            session.Flush();
        }

        public IGenericTransaction BeginTransaction()
        {
            return new GerericTransaction( session.BeginTransaction() );
        }

        // registration request is received, they should be performed serially.
        public IGenericTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new GerericTransaction( session.BeginTransaction( isolationLevel ) );
        }

        //method should flush the content of the NHibernate session to the 
        //database wrapped inside a transaction automatically assumes an isolation level equal to "read commited"
        public void TransactionalFlush()
        {
            //Specifies that statements cannot read data that has been modified but 
            //not committed by other transactions.
            TransactionalFlush( IsolationLevel.ReadCommitted );
        }

        //In the case of an exception during TransactionalFlush do not try to reuse the 
        //current unit of work since the session 
        //is in an inconsistent state and must be closed.
        public void TransactionalFlush(IsolationLevel isolationLevel)
        {
            //take this when making thread safe
            var tx = BeginTransaction( isolationLevel );
            try
            {
                //forces a flush of the current unit of work
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            finally
            {
                tx.Dispose();
            }
        }

        public void Dispose()
        {
            factory.DisposeUnitOfWork( this );
            if (session != null)
            {
                session.Dispose();
            }
            GC.SuppressFinalize( this );
        }
    }
}