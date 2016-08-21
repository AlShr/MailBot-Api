using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MailBot.DataAccessLayer.Mapping
{
    public class EmailAddressMapping : ClassMapping<EmailAddressDao>
    {
        public EmailAddressMapping()
        {
            Table( "emailAddress" );
            Lazy( true );
            Id( x => x.Id, x =>
            {
                x.Column( "id" );
                x.Generator( Generators.Native );
            } );
            Property( x => x.Address, x =>
            {
                x.Column( "address" );
                x.Length( 100 );
                x.NotNullable( true );
            } );
            ManyToOne( x => x.Owner, x =>
            {
                x.Column( "idUser" );
                x.Lazy( LazyRelation.NoProxy );
                x.NotNullable( false );
            } );
            OneToOne( x => x.VerificationKey, y =>
            {
                y.PropertyReference( typeof(VerificationKeyDao).GetProperty( "EmailAddress" ) );
                y.Cascade( Cascade.All );
            } );
            ManyToOne( x => x.GroupOwner, x =>
            {
                x.Column( "idGroup" );
                x.Unique(true);
                x.NotNullable( false );              
                x.ForeignKey("EmailAddress_Group_FK1");
            } );
            Set( x => x.RecipeMails, x =>
            {
                x.Table( "mailrecipient" );
                x.Key( y =>
                {
                    y.Column( "idEmail" );
                    y.NotNullable( true );
                } );
                x.Lazy( CollectionLazy.Lazy );
                x.Cascade( Cascade.All );
                x.Inverse( true );
            }, x => x.ManyToMany( y => y.Column( "idMail" ) ) );
            Set( x => x.Mails, x =>
            {
                x.Key( y =>
                {
                    y.Column( "idSender" );
                    y.NotNullable( true );
                } );
                x.Cascade( Cascade.All | Cascade.DeleteOrphans );
                x.Lazy( CollectionLazy.Lazy );
                x.Inverse( true );
            }, x => { x.OneToMany(); } );
        }
    }
}