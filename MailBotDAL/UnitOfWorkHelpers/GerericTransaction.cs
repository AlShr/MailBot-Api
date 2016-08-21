using NHibernate;

namespace MailBot.DataAccessLayer.UnitOfWorkHelpers
{
    public class GerericTransaction : IGenericTransaction
    {
        private readonly ITransaction transaction;

        public GerericTransaction(ITransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            transaction.Dispose();
        }
    }
}