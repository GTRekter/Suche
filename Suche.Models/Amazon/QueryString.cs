using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class QueryString
    {
        [JsonProperty("default_field")]
        public string DefaultField { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }
    }
}
