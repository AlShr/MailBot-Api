using System;
using System.Collections.Generic;
using MailBot.DataAccessLayer.DomainDAL;

namespace MailBot.DataAccessLayer.BusinessLayer.BusinessEntity
{
    public class UserEntity
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual ISet<EmailAddressEntity> Emails { get; set; }

        public static UserEntity ToBusinessItem(UserDao param)
        {
            if (param == null)
            {
                throw new ArgumentNullException( "param" );
            }
            var emails = new HashSet<EmailAddressEntity>();
            foreach (var email in param.Emails)
            {
                emails.Add( EmailAddressEntity.ToBussinessItem( email ) );
            }
            var businessItem = new UserEntity
            {
                Login = param.Login,
                Password = param.Password,
                Emails = emails
            };
            return businessItem;
        }
    }
}