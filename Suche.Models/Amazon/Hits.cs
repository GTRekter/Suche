using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class Hits<T>
    {

        [JsonProperty("total")]
        public Total Total { get; set; }

        [JsonProperty("max_score")]
        public double? MaxScore { get; set; }

        [JsonProperty("hits")]
        public List<HitsItem<T>> HitsItems { get; set; }
    }
}
