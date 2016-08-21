using System;

namespace MailBot.DataAccessLayer.UnitOfWorkHelpers
{
    public interface IGenericTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}