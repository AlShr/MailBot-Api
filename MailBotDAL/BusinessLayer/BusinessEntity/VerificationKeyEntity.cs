using System;
using MailBot.DataAccessLayer.DomainDAL;

namespace MailBot.DataAccessLayer.BusinessLayer.BusinessEntity
{
    public class VerificationKeyEntity
    {
        public virtual int Id { get; set; }
        public virtual string Key { get; set; }
        public virtual string Status { get; set; }
        public virtual EmailAddressEntity EmailAddress { get; set; }

        public static VerificationKeyEntity ToBussinessItem(VerificationKeyDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }

            var businessItem = new VerificationKeyEntity
            {
                Id = param.Id,
                Key = param.Key,
                Status = param.Status,
                EmailAddress = EmailAddressEntity.ToBussinessItem( param.EmailAddress )
            };
            return businessItem;
        }
    }
}