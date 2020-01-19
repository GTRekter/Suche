using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


namespace Suche.Models.Context
{
    public class ApartmentBuilding
    {
        [JsonProperty("propertyID")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("formerName")]
        public string FormerName { get; set; }

        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}