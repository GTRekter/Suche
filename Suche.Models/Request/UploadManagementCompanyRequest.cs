using Suche.Models.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Request
{
    public class UploadManagementCompanyRequest
    {
        public ManagementCompany[] ManagementCompany { get; set; }

        public string IndexName { get; set; }
    }
}
