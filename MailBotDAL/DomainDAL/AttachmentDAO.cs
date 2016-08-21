using System;
using System.Collections.Generic;

namespace MailBot.DataAccessLayer.DomainDAL
{
    public class AttachmentDao
    {
        public AttachmentDao()
        {
            Mails = new HashSet<MailDao>();
        }

        public virtual int Id { get; protected set; }
        public virtual byte[] Contents { get; set; }
        public virtual ISet<MailDao> Mails { get; protected set; }

        public virtual void AddMails(MailDao mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException( "mail" );
            }
            if (!Mails.Contains( mail ))
            {
                Mails.Add( mail );
                mail.AddAtachment( this );
            }
        }
    }
}