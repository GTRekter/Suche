using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class SearchQuery
    {
        [JsonProperty("from")]
        public int From { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        // TODO: Manage dynam properties
        //[JsonProperty("sort")]
        //public Sort Sort { get; set; }

        [JsonProperty("query")]
        public Query Query { get; set; }

        // TODO: Manage dynam properties
        //[JsonProperty("highlight")]
        //public Highlight Highlight { get; set; }
    }
}
