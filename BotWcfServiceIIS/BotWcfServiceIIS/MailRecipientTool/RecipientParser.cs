using System.Collections.Generic;
using System.Net.Mail;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.DomainDAL;
using System;

namespace MailBot.Service.MailRecipientTool
{
    //parse mailaddress and storage recipient to mail
    public interface IMailParser
    {
        MailEntity SetRecipient(MailEntity mail, MailMessage message);
        void SetMessageReferences(MailEntity mail, MailMessage message);
    }

    public abstract class MailInfoParser
    {
        public abstract IMailParser CreateMailParser();
    }

    public class BotMailParser : MailInfoParser
    {
        public override IMailParser CreateMailParser()
        {
            return new MailParser();
        }

        private class MailParser : IMailParser
        {
            public MailEntity SetRecipient(MailEntity mail, MailMessage message)
            {
                var resultCcBcc = new List<MailAddress>(message.CC);
                resultCcBcc.AddRange(message.Bcc);
                resultCcBcc.AddRange(message.To);
                var listaddress = new List<string>();
                foreach (var address in resultCcBcc)
                {
                    listaddress.Add( address.Address );
                }
                IEmailAddressDAL emailAddressDAL = new EmailAddressDao();
                var mailUpdate = emailAddressDAL.SaveRecipient( mail, listaddress );
                return mailUpdate;

            }
            public void SetMessageReferences(MailEntity mail, MailMessage message)
            {
                IMessageIdDAL messageDAL = new MessageIdDao();
                string messageId = message.Headers["Message-ID"];
                char[] delimeChars = { '<', '>' };
                string[] mailmessageId = messageId.Split(delimeChars, StringSplitOptions.RemoveEmptyEntries);
                messageDAL.SaveMessageId(mail, mailmessageId[0]);

                string childmessageIds = message.Headers["References"];
                if (childmessageIds != null)
                {
                    string[] hashes = childmessageIds.Split(delimeChars, StringSplitOptions.RemoveEmptyEntries);
                    messageDAL.SaveMessageId(mail, hashes);
                }
                messageDAL.SetParentMail(mail.Id);

            }      
        }
    }
}