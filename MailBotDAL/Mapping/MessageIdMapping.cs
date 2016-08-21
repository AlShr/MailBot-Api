using MailBot.DataAccessLayer.DomainDAL;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MailBot.DataAccessLayer.Mapping
{
    public class MessageIdMapping : ClassMapping<MessageIdDao>
    {
        public MessageIdMapping()
        {
            Table("messageIds");
            Lazy(true);
            Id(x => x.Id, x => 
            {
                x.Column("id");
                x.Generator(Generators.Native);
            });
            Property(x => x.Position, x => 
            {
                x.Column("position");
            });
            Property(x => x.MessageHash, x =>
            {
                x.Column("mailhash");
                x.Length(500);
            });
            ManyToOne(x => x.MailMessageId, x =>
            {
                x.Column("idMail");               
                x.NotNullable(false);
                x.Lazy(LazyRelation.Proxy);
                x.Cascade(Cascade.All);
            });           
        }
    }
}
