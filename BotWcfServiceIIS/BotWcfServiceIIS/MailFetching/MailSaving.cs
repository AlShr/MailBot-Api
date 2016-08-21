using System.Collections.Generic;
using System.Net.Mail;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using MailBot.DataAccessLayer.DomainDAL;
using MailBot.SearchIndexing;
using MailBot.Service.MailRecipientTool;
using S22.Imap;

namespace MailBot.Service.MailFetching
{
    public static class MailSaving
    {
        public static void ReactToNewMessage(object sender, IdleMessageEventArgs args)
        {
            var message = args.Client.GetMessage( args.MessageUID, FetchOptions.Normal );
            IEmailAddressDAL emailAddressDAL = new EmailAddressDao();
            var emailExist = emailAddressDAL.IsEmailAddressExists( message.From.Address );
            EmailAddressEntity emailSender = null;
            if (emailExist == false)
            {
                //todo: here important decision is to be taken.
                //either:
                //  we only add mails from users who already registered and linked their mailboxes to themselves
                //or:
                //  we add any mails that come into bot, however some of them might not be viewable before users
                //  these mails are addressed to register into MailBot.

                //ignoring for now, this is to be discussed.
                emailSender = emailAddressDAL.AddNewEmail( message.From.Address );
            }
            emailSender = emailAddressDAL.GetEmailAddressByName( message.From.Address );
            
            IMailDAL mailDAL = new MailDao();
            var mail = mailDAL.AddMail( message.Subject, message.Body, emailSender ); 

            MailInfoParser parserManager = new BotMailParser();
            var storage = parserManager.CreateMailParser();
            var mailEntity = storage.SetRecipient( mail, message);
            storage.SetMessageReferences(mail, message);


            //correct mail saving code goes here.
            LuceneIndex.Instance.Add( mailEntity );
        }
    }
}