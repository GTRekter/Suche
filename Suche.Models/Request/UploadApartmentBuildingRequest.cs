using Suche.Models.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Request
{
    public class UploadApartmentBuildingRequest
    {
        public ApartmentBuilding[] ApartmentBuilding { get; set; }

        public string IndexName { get; set; }
    }
}
