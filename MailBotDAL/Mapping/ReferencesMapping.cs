using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MailBot.DataAccessLayer.Mapping
{
    public class ReferencesMapping:ClassMapping<ReferencesDao>
    {
        public ReferencesMapping()
        {
            Table("referencesMail");
            Lazy(true);
            Id(x => x.Id, x =>
            {
                x.Column("id");
                x.Generator(Generators.Native);
            });
            ManyToOne(x => x.ChildMessageId, x =>
            {
                x.Column("ChildMessageId");
                x.Lazy(LazyRelation.Proxy);
                x.NotNullable(false);
            });
            ManyToOne(x => x.ParentMessageId, x =>
            {
                x.Column("ParentMessageId");
                x.Lazy(LazyRelation.Proxy);
                x.NotNullable(false);
            });
            ManyToOne(x => x.ParentMailId, x =>
            {
                x.Column("ParentMailId");
                x.Lazy(LazyRelation.Proxy);
                x.NotNullable(false);
            });
           
        }
    }
}
