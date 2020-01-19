using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class Query
    {
        // TODO: Handle this object which is not accepted as NULL from AWS
        //[JsonProperty("query_string")]
        //public QueryString QueryString { get; set; }

        [JsonProperty("multi_match")]
        public MultiMatch MultiMatch { get; set; }
    }
}
