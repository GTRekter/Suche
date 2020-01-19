using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Search;

namespace Suche.Models.Context
{
    public class Property
    {
        public int Id { get; set; }

        [Key]
        public string PropertyId { get; set; }

        [Required(ErrorMessage = "IdCategory is required")]
        public int IdCategory { get; set; }

        [IsSearchable, IsSortable]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [IsSearchable, IsSortable]
        [Required(ErrorMessage = "Size is required")]
        public float Size { get; set; }

        [IsSearchable, IsSortable]
        [Required(ErrorMessage = "Value is required")]
        public float Value { get; set; }

        [IsSearchable, IsSortable]
        public DateTime Year { get; set; }
    }
}
