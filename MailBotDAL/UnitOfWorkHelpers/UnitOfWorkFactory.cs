using System;
using System.IO;
using System.Xml;
using MailBot.DataAccessLayer.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Search.Store;
using NHibernate.Event;
using NHibernate.Search.Event;

namespace MailBot.DataAccessLayer.UnitOfWorkHelpers
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private const string DefaultHibernateConfig = "hibernate.cfg.xml";
        private readonly IUnitOfWorkImplementor implementor;
        private Configuration configuration;

        internal UnitOfWorkFactory()
        {
            SessionFactory = Configuration.BuildSessionFactory();
            implementor = new UnitOfWorkImplementor( this );
        }

        public ISessionFactory BuildSessionFactory()
        {
            var cfg = new Configuration().Configure();
            var sessionFactory = cfg.BuildSessionFactory();
            return sessionFactory;
        }
        /// <summary>
        /// !
        /// </summary>
        /// <returns></returns>
        private ISession CreateSession()
        {
            var session = SessionFactory.OpenSession();
            return WrapSession(session);
        }

        public IUnitOfWork Create()
        {
            var session = CreateSession();
            session.FlushMode = FlushMode.Commit;
            CurrentSession = session;
            ( (UnitOfWorkImplementor) implementor ).Session = CurrentSession;
            return implementor;
        }
        /// <summary>
        /// !
        /// </summary>
        /// <param name="cfg"></param>
        private void SetSearchProperty(Configuration cfg)
        {
            cfg.SetProperty(
                "hibernate.search.default.directory_provider",
                typeof(FSDirectoryProvider)
                .AssemblyQualifiedName);
            cfg.SetProperty(
                "hibernate.search.default.indexBase",
                "~/Index");
        }
        /// <summary>
        /// !
        /// </summary>
        /// <param name="cfg"></param>
        private void AddSearchListeners(Configuration cfg)
        {
            cfg.SetListener(ListenerType.PostUpdate,
                new FullTextIndexEventListener());
            cfg.SetListener(ListenerType.PostInsert,
                new FullTextIndexEventListener());
            cfg.SetListener(ListenerType.PostDelete,
                new FullTextIndexEventListener());
            cfg.SetListener(ListenerType.PostCollectionRecreate,
                new FullTextIndexCollectionEventListener());
            cfg.SetListener(ListenerType.PostCollectionRemove,
               new FullTextIndexCollectionEventListener());
            cfg.SetListener(ListenerType.PostCollectionUpdate,
               new FullTextIndexCollectionEventListener());
        }

        public Configuration Configuration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = new Configuration()
                        .DataBaseIntegration( db => db.SchemaAction = SchemaAutoAction.Update );
                    var hibernateConfig = DefaultHibernateConfig;
                    // if not rooted, assume path from base directory
                    if (Path.IsPathRooted( hibernateConfig ) == false)
                    {
                        hibernateConfig = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, hibernateConfig );
                    }
                    if (File.Exists( hibernateConfig ))
                    {
                        configuration.Configure( new XmlTextReader( hibernateConfig ) );
                    }
                    var mapper = new ModelMapper();
                    mapper.AddMapping<UserMapping>();
                    mapper.AddMapping<GroupMapping>();
                    mapper.AddMapping<EmailAddressMapping>();
                    mapper.AddMapping<MailMapping>();
                    mapper.AddMapping<VerificationKeyMapping>();
                    mapper.AddMapping<AttachmentMapping>();
                    mapper.AddMapping<MessageIdMapping>();
                    mapper.AddMapping<ReferencesMapping>();
                    var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
                    configuration.AddDeserializedMapping( mappings, null );
                    ///!
                    SetSearchProperty(configuration);
                    AddSearchListeners(configuration);

                }
                return configuration;
            }
        }
        /// <summary>
        /// !
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private static ISession WrapSession(ISession session)
        {
            return NHibernate.Search.Search.CreateFullTextSession(session);
        }

        public ISessionFactory SessionFactory { get; private set; }
        public ISession CurrentSession { get; set; }

        public void DisposeUnitOfWork(UnitOfWorkImplementor adapter)
        {
            CurrentSession = null;
            UnitOfWork.DisposeUnitOfWork( adapter );
        }
    }
}