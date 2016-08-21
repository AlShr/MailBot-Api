using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MailBot.DataAccessLayer.Mapping
{
    public class GroupMapping : ClassMapping<GroupDao>
    {
        public GroupMapping()
        {
            Table( "groups" );
            Lazy( true );
            Id( x => x.Id, x =>
            {
                x.Column( "id" );
                x.Generator( Generators.Native );
            } );
            Property( x => x.GroupName, x =>
            {
                x.Column( "groupName" );
                x.Length( 20 );
                x.NotNullable( true );
            } );

            Set( x => x.Users, x =>
            {
                x.Table( "usermembership" );
                x.Key( y =>
                {
                    y.Column( "idGroup" );
                    y.NotNullable( true );
                } );
                x.Lazy( CollectionLazy.Lazy );
                x.Cascade( Cascade.None );
            },
                x => x.ManyToMany( y => y.Column( "idUser" ) ) );
           OneToOne(x => x.GroupAddress, y =>
           {
               y.PropertyReference(typeof(EmailAddressDao).GetProperty("GroupOwner"));
               y.Cascade(Cascade.All);
           });                
        }
    }
}