using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using NHibernate;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public interface IGroupDAL
    {
        GroupEntity RegisterNewGroup(string groupName);
        GroupEntity FindGroupByName(string groupName);
      
    }
}