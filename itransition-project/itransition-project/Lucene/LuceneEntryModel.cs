using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Version = Lucene.Net.Util.Version;
using itransition_project.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace itransition_project.Lucene
{
    public static class LuceneEntryModel
    {
        private static string _luceneDir =
                                           Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "LuceneEntryIndex");
        private static FSDirectory _directoryTemp;
        private static FSDirectory _directory
        {
            get
            {
                if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }
        private static void AddToLuceneIndex(Comix entry, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", entry.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field("Id", entry.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Name", entry.Name, Field.Store.YES, Field.Index.ANALYZED));
            //doc.Add(new Field("AgeRating", entry.AgeRating.Type, Field.Store.YES, Field.Index.ANALYZED));
            string tags = "";
            foreach (var tag in entry.Tags)
            {
                tags += tag.Text + " ";
            }
            doc.Add(new Field("Tags", tags, Field.Store.YES, Field.Index.ANALYZED));

            string Balloon = "";
            foreach (var pages in entry.Pages)
            {
                foreach (var frames in pages.Frames)
                {
                    foreach (var balloons in frames.Balloons)
                    {
                            Balloon += balloons.Text + " ";
                    }
                }
            }
            doc.Add(new Field("Balloons", Balloon, Field.Store.YES, Field.Index.ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }
        public static void AddUpdateLuceneIndex(IEnumerable<Comix> entries)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)
                foreach (var entry in entries) AddToLuceneIndex(entry, writer);
                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }
        public static void AddUpdateLuceneIndex(Comix entry)
        {
            AddUpdateLuceneIndex(new List<Comix> { entry });
        }

        public static void ClearLuceneIndexRecord(int entryId)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", entryId.ToString()));
                writer.DeleteDocuments(searchQuery);
                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }
        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();
                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }
        private static Comix MapLuceneDocumentToData(Document doc)
        {
            return new Comix
            {
                Id = Convert.ToInt32(doc.Get("Id")),
                Name = doc.Get("Name")
            };
        }
        private static IEnumerable<Comix> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }
        private static IEnumerable<Comix> MapLuceneToDataList(IEnumerable<ScoreDoc> hits,
        IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }
        private static Query parseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }
        private static IEnumerable<Comix> _search(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<Comix>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] { "Id", "Name", "Tags", "Balloons", "Description", "Comments", "Tags" }, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search
                    (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
        public static IEnumerable<Comix> Search(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<Comix>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return _search(input, fieldName);
        }

        private static IEnumerable<Comix> _searchByTag(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<Comix>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] {"Tags"}, analyzer);
                    var query = parseQuery(searchQuery, parser);
                    var hits = searcher.Search
                    (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }

        public static IEnumerable<Comix> SearchByTag(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<Comix>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return _searchByTag(input, fieldName);
        }


    }
}