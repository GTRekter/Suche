using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class MultiMatch
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("fields")]
        public string[] Fields { get; set; }
    }
}
