using Newtonsoft.Json;
using Suche.Models.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models
{
    public class ApartmentBuildingFile
    {

        [JsonProperty("property")]
        public ApartmentBuilding ApartmentBuilding { get; set; }
    }
}
