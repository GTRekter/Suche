using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    public class Total
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("relation")]
        public string Relation { get; set; }
    }
}
