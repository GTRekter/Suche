using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suche.Models.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Suche.Controllers
{
    [Route("Properties")]
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext context;
        public PropertyController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("Get")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            { 
                IEnumerable<Property> properties = await context.GetPropertiesAsync();
                return Ok(properties);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [Route("GetById")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            try
            { 
                Property property = await context.GetPropertyByIdAsync(id);
                return Ok(property);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [Route("GetPropertiesByCategoryId")]
        [Authorize]
        public async Task<IActionResult> GetPropertiesByCategoryId(int id)
        {
            try
            {
                IEnumerable<Property> properties = await context.GetPropertiesByCategoryIdAsync(id);
                return Ok(properties);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}