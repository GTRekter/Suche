using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Elasticsearch;
using Amazon.Elasticsearch.Model;
using Amazon.Runtime;
using Newtonsoft.Json;

namespace Suche.Models.Amazon
{
    public class Search
    {
        private readonly AmazonElasticsearchClient client;
        [Obsolete]
        private readonly string serviceEndpoint;
        #region Constructor
        [Obsolete]
        public Search(string searchServiceEndpoint)
        {
            client = new AmazonElasticsearchClient();
            serviceEndpoint = searchServiceEndpoint;
        }
        public Search(string awsAccessKeyId, string awsSecretAccessKey, string region)
        {
            var regionEndpoint = RegionEndpoint.EnumerableAllRegions.Where(p => p.SystemName.Equals(region, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            client = new AmazonElasticsearchClient(awsAccessKeyId, awsSecretAccessKey, regionEndpoint);
        }
        [Obsolete] 
        public Search(string awsAccessKeyId, string awsSecretAccessKey, string region, string searchServiceEndpoint)
        {
            var regionEndpoint = RegionEndpoint.EnumerableAllRegions.Where(p => p.SystemName.Equals(region, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault() ;
            client = new AmazonElasticsearchClient(awsAccessKeyId, awsSecretAccessKey, regionEndpoint);
            serviceEndpoint = searchServiceEndpoint;
        }
        [Obsolete]
        public Search(string awsAccessKeyId, string awsSecretAccessKey, RegionEndpoint region, string searchServiceEndpoint)
        {
            client = new AmazonElasticsearchClient(awsAccessKeyId, awsSecretAccessKey, region);
            serviceEndpoint = searchServiceEndpoint;
        }
        #endregion
        #region Public methods
        public ListTagsResponse GetTags(string arn, List<Tag> tags)
        {
            ListTagsRequest request = new ListTagsRequest() { ARN = arn };
            return client.ListTagsAsync(request).GetAwaiter().GetResult();
        }
        public async Task<ListTagsResponse> ListTagsAsyncAsync(string arn)
        {
            ListTagsRequest request = new ListTagsRequest() { ARN = arn };
            return await client.ListTagsAsync(request);
        }
        public AddTagsResponse AddTags(string arn, List<Tag> tags)
        {
            AddTagsRequest request = new AddTagsRequest() { ARN = arn, TagList = tags };
            return client.AddTagsAsync(request).GetAwaiter().GetResult();
        }
        public async Task<AddTagsResponse> AddTagsAsync(string arn, List<Tag> tags)
        {
            AddTagsRequest request = new AddTagsRequest() { ARN = arn, TagList = tags };
            return await client.AddTagsAsync(request);
        }
        public RemoveTagsResponse RemoveTags(string arn, List<string> keys)
        {
            RemoveTagsRequest request = new RemoveTagsRequest() { ARN = arn, TagKeys = keys };
            return client.RemoveTagsAsync(request).GetAwaiter().GetResult();
        }
        public async Task<RemoveTagsResponse> RemoveTagsAsync(string arn, List<string> keys)
        {
            RemoveTagsRequest request = new RemoveTagsRequest() { ARN = arn, TagKeys = keys };
            return await client.RemoveTagsAsync(request);
        }
        public HttpResponseMessage UploadDocuments<T>(T document, string indexName, string domainName)
        {
            var domain = GetDomain(domainName);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(domain.DomainStatus.Endpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.PutAsync($"/{indexName}/_doc/1", httpContent).GetAwaiter().GetResult();
        }
        [Obsolete]
        public HttpResponseMessage UploadDocuments<T>(T document, string indexName)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceEndpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.PutAsync($"/{indexName}/_doc/1", httpContent).GetAwaiter().GetResult();
        }
        [Obsolete]
        public async Task<HttpResponseMessage> UploadDocumentsAsync<T>(T document, string indexName)
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceEndpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.PutAsync($"/{indexName}/_doc/1", httpContent);
        }
        public async Task<HttpResponseMessage> UploadDocumentsAsync<T>(T document, string indexName, string domainName)
        {
            var domain = GetDomain(domainName);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(document), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(domain.DomainStatus.Endpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.PutAsync($"/{indexName}/_doc/1", httpContent);
        }
        [Obsolete]
        public HttpResponseMessage UploadDocuments<T>(T[] documents, string indexName)
        {
            string bulk = BuildBulks<T>(documents, indexName);
            HttpContent httpContent = new StringContent(bulk, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceEndpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.PostAsync("/_bulk", httpContent).GetAwaiter().GetResult();
        }
        [Obsolete]
        public async Task<HttpResponseMessage> UploadDocumentsAsync<T>(T[] documents, string indexName)
        {
            string bulk = BuildBulks<T>(documents, indexName);
            HttpContent httpContent = new StringContent(bulk, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceEndpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.PostAsync("/_bulk", httpContent);
        }
        public HttpResponseMessage UploadDocuments<T>(T[] documents, string indexName, string domainName)
        {
            var domain = GetDomain(domainName);
            string bulk = BuildBulks<T>(documents, indexName);
            HttpContent httpContent = new StringContent(bulk, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.PostAsync("/_bulk", httpContent).GetAwaiter().GetResult();
        }
        public async Task<HttpResponseMessage> UploadDocumentsAsync<T>(T[] documents, string indexName, string domainName)
        {
            var domain = GetDomain(domainName);
            string bulk = BuildBulks<T>(documents, indexName);
            HttpContent httpContent = new StringContent(bulk, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.PostAsync("/_bulk", httpContent);
        }
        public IEnumerable<T> SearchDocument<T>(string query, string domainName, string indexName)
        {
            var domain = GetDomain(domainName);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            var result = client.GetAsync($"{indexName}/_search?q={query}").GetAwaiter().GetResult();
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<SearchResponse<T>>(content).Hits.HitsItems.Select(i => i.Source);
            }
            else
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
        }
        public async Task<IEnumerable<T>> SearchDocumentAsync<T>(string query, string domainName, string indexName)
        {
            var domain = GetDomain(domainName);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            var result = await client.GetAsync($"{indexName}/_search?q={query}");
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchResponse<T>>(content).Hits.HitsItems.Select(i => i.Source);
            }
            else
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
        }
        public IEnumerable<T> SearchDocument<T>(SearchQuery query, string indexName, string domainName)
        {
            var domain = GetDomain(domainName);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = client.PostAsync($"/{indexName}/_search", httpContent).GetAwaiter().GetResult();
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<SearchResponse<T>>(content).Hits.HitsItems.Select(i => i.Source);
            }
            else
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }

        }
        public async Task<IEnumerable<T>> SearchDocumentAsync<T>(SearchQuery query, string indexName, string domainName)
        {
            var domain = GetDomain(domainName);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result =  await client.PostAsync($"/{indexName}/_search", httpContent);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchResponse<T>>(content).Hits.HitsItems.Select(i => i.Source);
            }
            else
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
        }
        public IEnumerable<T> SearchDocument<T>(SearchQuery query, string domainName)
        {
            var domain = GetDomain(domainName);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = client.PostAsync($"/_search", httpContent).GetAwaiter().GetResult();
            if (result.IsSuccessStatusCode)
            {
                var content = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<SearchResponse<T>>(content).Hits.HitsItems.Select(i => i.Source);
            }
            else
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
        }
        public async Task<IEnumerable<T>> SearchDocumentAsync<T>(SearchQuery query, string domainName)
        {
            var domain = GetDomain(domainName);
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("https://{0}", domain.DomainStatus.Endpoint));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.PostAsync($"/_search", httpContent);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchResponse<T>>(content).Hits.HitsItems.Select(i => i.Source);
            }
            else
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
        }
        #endregion
        #region Private methods
        /// <summary>
        /// This method retun create a valid string with a valid bulk structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        private string BuildBulks<T>(T[] document, string indexName)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < document.Length; i++)
            {
                builder.AppendLine(JsonConvert.SerializeObject(new { index = new Index() { Id = (i + 1).ToString(), Name = indexName, Type = "_doc" }}));
                builder.AppendLine(JsonConvert.SerializeObject(document[i]));
            }
            return builder.ToString();
        }
        private DescribeElasticsearchDomainResponse GetDomain(string domainName)
        {
            DescribeElasticsearchDomainRequest request = new DescribeElasticsearchDomainRequest() { DomainName = domainName };
            return client.DescribeElasticsearchDomainAsync(request).GetAwaiter().GetResult();
        }
        private async Task<DescribeElasticsearchDomainResponse> GetDomainAsync(string domainName)
        {
            DescribeElasticsearchDomainRequest request = new DescribeElasticsearchDomainRequest() { DomainName = domainName };
            return await client.DescribeElasticsearchDomainAsync(request);
        }
        #endregion
    }
}
