using System;
using NHibernate.Cfg;

namespace MailBot.DataAccessLayer.UnitOfWorkHelpers
{
    //wrappper around a NHibernate session object
    public static class UnitOfWork
    {
        private static readonly IUnitOfWorkFactory unitOfWorkFactory = new UnitOfWorkFactory();
        private static IUnitOfWork innerUnitOfWork;

        public static Configuration Configuration
        {
            get { return unitOfWorkFactory.Configuration; }
        }

        public static IUnitOfWork Current
        {
            get
            {
                if (innerUnitOfWork == null)
                {
                    throw new InvalidOperationException( "You are not in unit of work." );
                }
                return innerUnitOfWork;
            }
            set { innerUnitOfWork = value; }
        }

        public static bool IsStarted
        {
            get { return innerUnitOfWork != null; }
        }

        public static IUnitOfWork Start()
        {
            innerUnitOfWork = unitOfWorkFactory.Create();
            return innerUnitOfWork;
        }

        public static void DisposeUnitOfWork(UnitOfWorkImplementor adapter)
        {
            Current = null;
        }
    }
}