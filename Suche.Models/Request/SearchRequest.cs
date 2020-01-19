using Suche.Models.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Request
{
    public class SearchRequest
    {
        public SearchRequest()
        {
            Limit = 25;
        }
        public string Phase { get; set; }
        public IEnumerable<string> Market { get; set; }
        public int Limit { get; set; }
    }
}
