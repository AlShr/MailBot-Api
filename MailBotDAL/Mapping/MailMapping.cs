using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MailBot.DataAccessLayer.Mapping
{
    public class MailMapping : ClassMapping<MailDao>
    {
        public MailMapping()
        {
            Table( "mails" );
            Lazy( true );
            Id( x => x.Id, x =>
            {
                x.Column( "id" );
                x.Generator( Generators.Native );
            } );
            Property( x => x.Subject, x =>
            {
                x.Column( "mailsubject" );
                x.Length( 200 );
            } );
            Property( x => x.Body, x =>
            {
                x.Column( "mailbody" );
                x.Length( 1000 );
            } );
            Property(x => x.ParentId, x => 
            {
                x.Column("ParentId");
                x.NotNullable(false);
            });
            Property(x => x.MessageId, x => 
            {
                x.Column("messageId");
                x.NotNullable(true);
            });
           
            OneToOne(x => x.ReferencesMailRecord, y =>
            {
                y.PropertyReference(typeof(ReferencesDao).GetProperty("ReferencesMailRecord"));
                y.Cascade(Cascade.All);
            });
            Set(x => x.Children, x =>
                {
                    x.Key(z =>
                    {
                        z.Column("parentId");
                    });
                    x.Lazy(CollectionLazy.Lazy);
                    x.Cascade(Cascade.All);
                    x.Inverse(false);
                },
                x => x.OneToMany()); 
            ManyToOne( x => x.Sender, x =>
            {
                x.Column( "idSender" );
                x.Lazy( LazyRelation.Proxy );
                x.NotNullable( true );
            } );
            Set( x => x.Recipients, x =>
            {
                x.Table( "mailrecipient" );
                x.Key( y =>
                {
                    y.Column( "idMail" );
                    y.NotNullable( true );
                } );
                x.Lazy( CollectionLazy.Lazy );
                x.Cascade( Cascade.All );
            },
                x => x.ManyToMany( y => y.Column( "idEmail" ) ) );
            Set( x => x.Attachments, x =>
            {
                x.Table( "mailattachments" );
                x.Key( y =>
                {
                    y.Column( "idMail" );
                    y.NotNullable( true );
                } );
                x.Cascade( Cascade.All | Cascade.DeleteOrphans );
                x.Lazy( CollectionLazy.Lazy );
            }, x => x.ManyToMany( y => y.Column( "idAttachment" ) ) );
            List(x => x.MailMessageIds, x => 
            {
                x.Key(y => y.Column("idMail"));
                x.Index(y => y.Column("position"));
                x.Cascade(Cascade.All | Cascade.DeleteOrphans);
                x.Lazy(CollectionLazy.Lazy);
                x.Inverse(true);
            }, x => { x.OneToMany(); });
        }
    }
}