using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class Index
    {
        [JsonProperty("_index")]
        public string Name { get; set; }

        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }
}
