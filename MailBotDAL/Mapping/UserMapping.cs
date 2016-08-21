using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MailBot.DataAccessLayer.Mapping
{
    public class UserMapping : ClassMapping<UserDao>
    {
        public UserMapping()
        {
            Table( "users" );
            Lazy( true );
            Id( x => x.Id, x =>
            {
                x.Column( "id" );
                x.Generator( Generators.Native );
            } );
            Property( x => x.Login, x =>
            {
                x.Column( "login" );
                x.Length( 20 );
                x.NotNullable( true );
            } );
            Property( x => x.Password, x =>
            {
                x.Column( "pass" );
                x.Length( 100 );
                x.NotNullable( true );
            } );
            Set( x => x.Groups, x =>
            {
                x.Table( "usermembership" );
                x.Key( y =>
                {
                    y.Column( "idUser" );
                    y.NotNullable( true );
                } );
                x.Lazy( CollectionLazy.Lazy );
                x.Cascade( Cascade.All | Cascade.DeleteOrphans );
                x.Inverse( true );
            },
                x => x.ManyToMany( y => y.Column( "idGroup" ) ) );
            Set( x => x.Emails, x =>
            {
                x.Key( y =>
                {
                    y.Column( "idUser" );
                    y.NotNullable( true );
                } );
                x.Cascade( Cascade.All | Cascade.DeleteOrphans );
                x.Lazy( CollectionLazy.Lazy );
                x.Inverse( true );
            }, x => { x.OneToMany(); } );
        }
    }
}