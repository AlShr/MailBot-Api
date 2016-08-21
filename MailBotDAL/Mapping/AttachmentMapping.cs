using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace MailBot.DataAccessLayer.Mapping
{
    public class AttachmentMapping : ClassMapping<AttachmentDao>
    {
        public AttachmentMapping()
        {
            Table( "Attachment" );
            Lazy( true );
            Id( x => x.Id, x =>
            {
                x.Column( "id" );
                x.Generator( Generators.Native );
            } );
            Property( x => x.Contents, x =>
            {
                x.Column( "attachment" );

                x.Type<BinaryBlobType>();
                x.NotNullable( true );
                x.Lazy( true );
            } );
            Set( x => x.Mails, x =>
            {
                x.Table( "mailattachments" );
                x.Key( y =>
                {
                    y.Column( "idAttachment" );
                    y.NotNullable( true );
                } );
                x.Cascade( Cascade.All | Cascade.DeleteOrphans );
                x.Lazy( CollectionLazy.Lazy );
                x.Inverse( true );
            }, x => x.ManyToMany( y => y.Column( "idMail" ) ) );
        }
    }
}