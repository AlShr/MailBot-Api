using System;
using System.Data;

namespace MailBot.DataAccessLayer.UnitOfWorkHelpers
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericTransaction BeginTransaction();
        IGenericTransaction BeginTransaction(IsolationLevel isolationLevel);
        void TransactionalFlush();
        void TransactionalFlush(IsolationLevel isolationLevel);
    }
}