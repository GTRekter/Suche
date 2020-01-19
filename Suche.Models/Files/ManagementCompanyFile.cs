using Newtonsoft.Json;
using Suche.Models.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models
{
    public class ManagementCompanyFile
    {

        [JsonProperty("mgmt")]
        public ManagementCompany Company { get; set; }
    }
}
