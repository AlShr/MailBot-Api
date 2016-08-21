using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MailBot.DataAccessLayer.Mapping
{
    public class VerificationKeyMapping : ClassMapping<VerificationKeyDao>
    {
        public VerificationKeyMapping()
        {
            Table( "verificationKey" );

            Id( x => x.Id, x =>
            {
                x.Column( "id" );
                x.Generator( Generators.Native );
            } );
            Property( x => x.Key, x =>
            {
                x.Column( "keys" );
                x.Length( 32 );
                x.NotNullable( true );
            } );
            Property( x => x.Status, x =>
            {
                x.Column( "status" );
                x.Length( 20 );
                x.NotNullable( true );
            } );
            ManyToOne( x => x.EmailAddress,
                x =>
                {
                    x.Column( "idEmail" );
                    x.Unique( true );
                    x.NotNullable(false);
                    x.ForeignKey( "EmailAddress_Verify_FK1" );
                } );
        }
    }
}