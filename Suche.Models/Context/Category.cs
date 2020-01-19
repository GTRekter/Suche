using Microsoft.Azure.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Suche.Models.Context
{
    public class Category
    {
        public int Id { get; set; }

        [Key]
        [IsSearchable]
        public string CategoryId { get; set; }

        [IsSearchable, IsFacetable]
        [Required(ErrorMessage = "Title is required")]
        [StringLength(60, ErrorMessage = "Title can't be longer than 60 characters")]
        public string Title { get; set; }
    }
}
