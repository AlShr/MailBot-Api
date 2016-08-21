using System;
using MailBot.DataAccessLayer.DomainDAL;

namespace MailBot.DataAccessLayer.BusinessLayer.BusinessEntity
{
    public class GroupEntity
    {
        public virtual int Id { get; set; }
        public virtual string GroupName { get; set; }
        public virtual EmailAddressEntity GroupAddress { get; set; }

        public static GroupEntity ToBusinessItem(GroupDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var businessItem = new GroupEntity
            {
                Id = param.Id,
                GroupName = param.GroupName,
                GroupAddress = param.GroupAddress == null ? null: EmailAddressEntity.ToBussinessItem( param.GroupAddress )
            };
            return businessItem;
        }
    }
}