using Newtonsoft.Json;

namespace Suche.Models.Amazon
{
    public class Highlight
    {
        [JsonProperty("pre_tags")]
        public string PreTags { get; set; }

        [JsonProperty("post_tags")]
        public string PostTags { get; set; }

        [JsonProperty("fragment_size")]
        public string FragmentSize { get; set; }

        [JsonProperty("boundary_chars")]
        public string BoundaryChars { get; set; }
    }
}