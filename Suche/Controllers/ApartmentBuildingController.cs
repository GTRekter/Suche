using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Suche.Models;
using Suche.Models.Amazon;
using Suche.Models.Context;
using Suche.Models.Request;
using Newtonsoft.Json;

namespace Suche.Controllers
{
    [Route("ApartmentBuildings")]
    public class ApartmentBuildingController : Controller
    {
        private readonly string region;
        private readonly string accessKeyId;
        private readonly string secretAccessKey;
        private readonly string domainName;

        public ApartmentBuildingController(IConfiguration configuration)
        {
            this.region = configuration["Amazon:Region"];
            this.accessKeyId = configuration["Amazon:AccessKeyId"];
            this.secretAccessKey = configuration["Amazon:SecretAccessKey"];
            this.domainName = configuration["Amazon:DomainName"];
        }

        [HttpPost]
        [Route("UpdateDocuments")]
        [Consumes("application/json")]
        [Produces("application/json")]
        //[Authorize]
        public async Task<IActionResult> UpdateDocuments([FromBody]UploadApartmentBuildingRequest request)
        {
            try
            {
                Debug.WriteLine($"Apartment building: {request.ApartmentBuilding}");
                Debug.WriteLine($"IndexName: {request.IndexName}");
                Search search = new Search(accessKeyId, secretAccessKey, region);
                HttpResponseMessage result = await search.UploadDocumentsAsync<ApartmentBuilding>(request.ApartmentBuilding, request.IndexName, domainName);
                if(!result.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(result.ReasonPhrase);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Search")]
        [Consumes("application/json")]
        [Produces("application/json")]
        //[Authorize]
        public async Task<IActionResult> Search([FromBody]SearchRequest request)
        {
            try
            {
                Debug.WriteLine($"Phase: {request.Phase}");
                Debug.WriteLine($"Market: {string.Join(',', request.Market)}");
                Debug.WriteLine($"Limit: {request.Limit}");
                Search search = new Search(accessKeyId, secretAccessKey, region);

                string[] fields = GetFieldsByType<ApartmentBuilding>(typeof(String));
                SearchQuery searchQuery = BuildQuery(request, fields);

                // Unfortunately I didn't find any infor about multi-filter usage in a search query
                var result = await search.SearchDocumentAsync<ApartmentBuilding>(searchQuery, domainName);
                // On the result are filtered by Phare i execute another filter by market
                if(request.Market.Count() > 0)
                {
                    return Ok(result.Where(r => request.Market.Contains(r.Market)).ToList());
                }
                return Ok(result.ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        #region Private Methods
        /// <summary>
        /// TODO: Create a generic methos in order to avoid duplicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetFieldsByType<T>(Type type)
        {
            List<string> fields = new List<string>();
            Type objectType = typeof(T);
            foreach (var prop in objectType.GetProperties())
            {
                if (type.IsAssignableFrom(prop.PropertyType))
                {
                    object[] attributes = prop.GetCustomAttributes(true);
                    // I had to check the Json property name because AWS is key sensitive 
                    string propertyName = (attributes.Where(a => a as JsonPropertyAttribute != null).FirstOrDefault() as JsonPropertyAttribute)?.PropertyName;
                    fields.Add(propertyName);
                }
            }
            return fields.ToArray();
        }
        /// <summary>
        /// TODO: Create a generic methos in order to avoid duplicates
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private SearchQuery BuildQuery(SearchRequest request, string[] fields)
        {
            return new SearchQuery()
            {
                Size = request.Limit,
                Query = new Query() { MultiMatch = new MultiMatch() { Query = request.Phase, Fields = fields } }
            };
        }
        #endregion
    }
}