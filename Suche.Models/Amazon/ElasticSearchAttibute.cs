using System;
using System.Collections.Generic;
using System.Text;

namespace Suche.Models.Amazon
{
    [AttributeUsage(AttributeTargets.All)]
    public class ElasticSearchAttibute : Attribute
    {
        public ElasticSearchType Type { get; set; }
        public ElasticSearchAttibute(ElasticSearchType type)
        {
            this.Type = type;
        }
    }
}
