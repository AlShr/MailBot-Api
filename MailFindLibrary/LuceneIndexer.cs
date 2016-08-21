using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using MailBot.DataAccessLayer.BusinessLayer.BusinessEntity;
using Directory = Lucene.Net.Store.Directory;
using Version = Lucene.Net.Util.Version;

namespace MailBot.SearchIndexing
{
    public class LuceneIndex
    {
        private readonly Directory indexDirectory;

        public void UpdateIndex(IEnumerable<MailEntity> newMails)
        {
            AddRange( newMails );
        }

        public void Add(MailEntity mail)
        {
            using (var writer = new IndexWriter( indexDirectory, new StandardAnalyzer( Version.LUCENE_30 ), IndexWriter.MaxFieldLength.UNLIMITED ))
            {
                var searchQuery = new TermQuery( new Term( "Id", mail.Id.ToString() ) );
                writer.DeleteDocuments( searchQuery );
                writer.AddDocument( LuceneConvert.ToDocument( mail ) );
            }
        }

        public void AddRange(IEnumerable<MailEntity> mails)
        {
            foreach (var mail in mails)
            {
                Add( mail );
            }
        }

        private Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse( searchQuery.Trim() );
            }
            catch (ParseException)
            {
                query = parser.Parse( QueryParser.Escape( searchQuery.Trim() ) );
            }
            return query;
        }

        private IEnumerable<int> SearchMails(string searchQuery, string[] fields = null)
        {
            if (( ( fields == null ) || ( fields.Length == 0 ) ) ||
                string.IsNullOrEmpty( searchQuery.Replace( "*", "" ).Replace( "?", "" ) ))
            {
                //todo: exeption or empty list?
                //throw new ApplicationException("You cant search in noone field!");
                return new List<int>();
            }
            // set up lucene searcher
            using (var searcher = new IndexSearcher( indexDirectory, false ))
            {
                var hitsLimit = 1000;
                var analyzer = new StandardAnalyzer( Version.LUCENE_30 );
                //search by multiplie fields
                var parser = new MultiFieldQueryParser
                    ( Version.LUCENE_30, fields, analyzer );
                var query = ParseQuery( searchQuery, parser );
                var hits = searcher.Search
                    ( query, null, hitsLimit, Sort.RELEVANCE );
                var kek = hits.ScoreDocs;
                var results = LuceneConvert.ToMailIdList( kek, searcher );
                analyzer.Close();
                searcher.Dispose();
                return results;
            }
        }

        public IEnumerable<int> Search(string input, string[] fields = null)
        {
            if (string.IsNullOrEmpty( input ))
            {
                return new List<int>();
            }

            var terms = input.Trim().Replace( "-", " " ).Split( ' ' )
                .Where( x => !string.IsNullOrEmpty( x ) ).Select( x => x.Trim() );
            input = string.Join( " ", terms );
            return SearchMails( input, fields );
        }

        public IEnumerable<int> GetAllRecords()
        {
            // validate search index
            if (indexDirectory.ListAll().Length == 0)
            {
                return new List<int>();
            }
            // set up lucene searcher
            var searcher = new IndexSearcher( indexDirectory, false );
            var reader = IndexReader.Open( indexDirectory, false );
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next())
            {
                docs.Add( searcher.Doc( term.Doc ) );
            }
            reader.Dispose();
            searcher.Dispose();
            return LuceneConvert.ToMailIdList( docs );
        }

        #region Singleton implementation

        private static LuceneIndex instance;

        public static LuceneIndex Instance
        {
            get { return instance ?? ( instance = new LuceneIndex() ); }
        }

        private LuceneIndex()
        {
            var direcory = Environment.ExpandEnvironmentVariables( "%USERPROFILE%\\Lucene.dir" );
            if (!System.IO.Directory.Exists( direcory ))
            {
                System.IO.Directory.CreateDirectory( direcory );
            }
            indexDirectory = new SimpleFSDirectory( new DirectoryInfo( direcory ) );
        }

        #endregion
    }
}