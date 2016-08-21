using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;

namespace MailBot.SearchIndexing
{
    public static class LuceneConvert
    {
        //todo: as (Sender and Recipient) email or nickname?
        public static Document ToDocument(MailEntity mail)
        {
            var document = new Document();
            document.Add( new Field( "Id", mail.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED ) );
            document.Add( new Field( "Subject", mail.Subject, Field.Store.YES, Field.Index.ANALYZED ) );
            document.Add( new Field( "Body", mail.Body, Field.Store.YES, Field.Index.ANALYZED ) );
            document.Add( new Field( "Sender", mail.Sender.Address, Field.Store.YES, Field.Index.ANALYZED ) );
            document.Add( new Field( "Recipient", RecipientToString( mail.Recipients ), Field.Store.YES, Field.Index.ANALYZED ) );
            return document;
        }

        public static List<Document> ToDocumentList(IEnumerable<MailEntity> mails)
        {
            return mails.Select( ToDocument ).ToList();
        }

        private static int ToMailId(Document document)
        {
            return int.Parse( document.GetField( "Id" ).StringValue );
        }

        public static IEnumerable<int> ToMailIdList(IEnumerable<Document> documents)
        {
            return documents.Select( ToMailId ).ToList();
        }

        public static IEnumerable<int> ToMailIdList(IEnumerable<ScoreDoc> hits, Searchable searcher)
        {
            return hits.Select( hit => ToMailId( searcher.Doc( hit.Doc ) ) ).ToList();
        }

        public static string SenderToString(UserEntity sender)
        {
            return sender.Emails.Aggregate( string.Empty, (current, email) => current + email.Address + ", " );
        }

        private static string RecipientToString(IEnumerable<EmailAddressEntity> recipients)
        {
            return recipients == null
                ? string.Empty
                : recipients.Aggregate( string.Empty, (current, email) => current + email.Address + ", " );
        }
    }
}