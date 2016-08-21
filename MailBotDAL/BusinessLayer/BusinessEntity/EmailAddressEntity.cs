using System;
using MailBot.DataAccessLayer.DomainDAL;

namespace MailBot.DataAccessLayer.BusinessLayer.BusinessEntity
{
    public class EmailAddressEntity
    {
        public virtual string Address { get; set; }
        public virtual UserEntity Owner { get; set; }
        public virtual GroupEntity GroupOwner { get; set; }
        public virtual VerificationKeyEntity VerificationKey { get; set; }


        public static EmailAddressEntity ToBussinessItem(EmailAddressDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("EmailAddressDao");
            }
            var userBussinessItem = new UserEntity
            {
                Login = param.Owner == null ? null : param.Owner.Login,
                Password = param.Owner == null ? null : param.Owner.Password
            };
            //var groupBussinessItem = new GroupEntity
            //{
            //    GroupName = param.GroupOwner == null ? null : param.GroupOwner.GroupName,
            //    GroupAddress = param == null ? null : emailBussinessItem
            //};
            var verificationItem = new VerificationKeyEntity()
            {
                Key = param.VerificationKey == null ? null : param.VerificationKey.Key,
                Status = param.VerificationKey == null ? null : param.VerificationKey.Status
            };
            var emailBussinessItem = new EmailAddressEntity
            {
                Address = param.Address,
                Owner = userBussinessItem,
                //GroupOwner = groupBussinessItem,
                VerificationKey = verificationItem
            };
            emailBussinessItem.GroupOwner = new GroupEntity
            {
                GroupName = param.GroupOwner == null ? null : param.GroupOwner.GroupName,
                GroupAddress = param == null ? null : emailBussinessItem
            };
            return emailBussinessItem;
        }
    }
}