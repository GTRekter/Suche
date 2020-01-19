using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class HitsItem<T>
    {
        [JsonProperty("_index")]
        public string Index { get; set; }

        [JsonProperty("_type")]
        public string Type { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_score")]
        public double Score { get; set; }

        [JsonProperty("_source")]
        public T Source { get; set; }
    }
}
