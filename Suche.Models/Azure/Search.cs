using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Suche.Models.Azure
{
    public class Search
    {
        private readonly SearchServiceClient client;
        #region Constructor
        public Search(string searchServiceName, string adminApiKey)
        {
            client = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
        }
        #endregion
        #region Public methods
        public void CreateIndex<T>(string indexName)
        {
            var definition = new Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };
            client.Indexes.Create(definition);
        }
        public void UploadDocuments<T>(string indexName, T[] documents)
        {
            try
            {
                ISearchIndexClient indexClient = client.Indexes.GetClient(indexName);
                var actions = new List<IndexAction<T>>();
                for (int i = 0; i < documents.Length; i++)
                {
                    actions.Add(IndexAction.Upload(documents[i]));
                }
                var batch = IndexBatch.New(actions);
                indexClient.Documents.Index(batch);
            }
            catch (IndexBatchException e)
            {
                Debug.WriteLine("Failed to index some of the documents: {0}", String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }
        }
        public void DeleteIndexIfExists(string indexName)
        {
            if (client.Indexes.Exists(indexName))
            {
                client.Indexes.Delete(indexName);
            }
        }
        public IEnumerable<T> RunQuery<T>(string indexName, string serachText, SearchParameters parameters)
        {
            ISearchIndexClient indexClient = client.Indexes.GetClient(indexName);
            DocumentSearchResult<T> searchResults = indexClient.Documents.Search<T>(serachText, parameters);
            return searchResults.Results.Select(d => d.Document);
        }
        #endregion
    }
}
