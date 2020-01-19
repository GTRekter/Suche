using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Suche.Models;
using Suche.Models.Azure;
using Suche.Models.Context;
using Suche.Models.Request;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Suche.Controllers
{
    [Route("Categories")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly string searchServiceName;
        private readonly string adminApiKey;
        private readonly string indexName;

        public string Domain { get; set; }
        public CategoryController(IConfiguration configuration, ApplicationDbContext context)
        {
            this.context = context;
            this.searchServiceName = configuration["Azure:SearchServiceName"];
            this.adminApiKey = configuration["Azure:SearchServiceAdminApiKey"];
            this.indexName = configuration["Azure:SearchIndexName"];
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<Category> categories = await context.GetCategoriesAsync();
                Debug.WriteLine("Categories informations");
                Debug.WriteLine($"Count: {categories.Count()}");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }           
        }

        [HttpGet("{order}")]
        [Authorize]
        public async Task<IActionResult> Get(OrderType order)
        {
            try
            {
                IEnumerable<Category> categories = await context.GetCategoriesAsync(order);
                Debug.WriteLine("Categories informations");
                Debug.WriteLine($"Count: {categories.Count()}");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
                Category category = await context.GetCategoryByIdAsync(id);
                if (category != null)
                {
                    Debug.WriteLine("Category informations");
                    Debug.WriteLine($"Id: {category.Id}");
                    Debug.WriteLine($"Title: {category.Title}");
                }
                Debug.WriteLine("Category not found");
                return Ok(category);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Search")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> PostSearch([FromBody] SearchCategoryRequest request)
        {
            try
            {
                Search search = new Search(searchServiceName, adminApiKey);
                search.DeleteIndexIfExists(indexName);
                search.CreateIndex<Category>(indexName);

                IEnumerable<Category> categories = await context.GetCategoriesAsync();
                search.UploadDocuments<Category>(indexName, categories.ToArray());

                IEnumerable<Category> result = search.RunQuery<Category>(indexName, request.Text, null);
                Debug.WriteLine("Categories informations");
                Debug.WriteLine($"Count: {result.Count()}");
                return Ok(result.ToList());
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddCategoryJson")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> PostJson([FromBody] Category category)
        {
            try
            { 
                await context.AddCategoryAsync(category);
                Debug.WriteLine("Add category informations");
                Debug.WriteLine($"Id: {category.Id}");
                Debug.WriteLine($"Title: {category.Title}");
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddCategoryXml")]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        [Authorize]
        public async Task<IActionResult> PostXml([FromBody] Category category)
        {
            try
            {
                await context.AddCategoryAsync(category);
                Debug.WriteLine("Add category informations");
                Debug.WriteLine($"Id: {category.Id}");
                Debug.WriteLine($"Title: {category.Title}");
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Route("UpdateCategory")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Category category)
        {
            try
            {
                await context.UpdateCategoryAsync(id, category);
                Debug.WriteLine("Update category informations");
                Debug.WriteLine($"Id: {id}");
                Debug.WriteLine($"Title: {category.Title}");
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Route("DeleteCategory")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await context.DeleteCategoryByIdAsync(id);
                Debug.WriteLine("Delete category informations");
                Debug.WriteLine($"Id: {id}");
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}