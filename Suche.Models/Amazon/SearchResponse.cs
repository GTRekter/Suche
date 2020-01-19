using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class SearchResponse<T>
    {
        [JsonProperty("took")]
        public int Took { get; set; }

        [JsonProperty("timed_out")]
        public string TimedOut { get; set; }

        [JsonProperty("_shards")]
        public Shards Shards { get; set; }

        [JsonProperty("hits")]
        public Hits<T> Hits { get; set; }
    }
}
