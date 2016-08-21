using System;
using System.Collections.Generic;
using MailBot.DataAccessLayer.DomainDAL;

namespace MailBot.DataAccessLayer.BusinessLayer.BusinessEntity
{
    public class MailEntity
    {
        public virtual int Id { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual int MessageId { set; get; }
        public virtual EmailAddressEntity Sender { get; set; }
        public virtual IList<EmailAddressEntity> Recipients { get; set; }

        public static MailEntity ToBusinessItem(MailDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var emailAddrssBussinessItem = EmailAddressEntity.ToBussinessItem( param.Sender );
            var businessItem = new MailEntity
            {
                Id = param.Id,
                Subject = param.Subject,
                Body = param.Body,
                Sender = emailAddrssBussinessItem
            };
            var emails = new List<EmailAddressDao>( param.Recipients );
            var email = new EmailAddressEntity();
            businessItem.Recipients = new List<EmailAddressEntity>();
            emails.ForEach( x => businessItem.Recipients.Add( EmailAddressEntity.ToBussinessItem( x ) ) );
            return businessItem;
        }
    }
}