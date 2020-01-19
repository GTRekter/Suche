using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Suche.Models.Context
{
    public class ApplicationDbContext : DbContext
    {
        #region Properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Property> Properties { get; set; }
        #endregion
        #region Constructor
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        {
        }
        #endregion
        #region Public methods
        public static void Initialize(DbContextOptions options)
        {
            PopolateContext(options);
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            PopolateContext(options);
        }
        public IEnumerable<Category> GetCategories()
        {
            return Categories.ToList();
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await Categories.ToListAsync<Category>();
        }
        public IEnumerable<Category> GetCategories(OrderType order)
        {
            switch (order)
            {
                case OrderType.Asc:
                    return Categories.OrderBy(c => c.Title).ToList();
                case OrderType.Desc:
                    return Categories.OrderByDescending(c => c.Title).ToList();
                default:
                    return Categories.ToList();
            }
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync(OrderType order)
        {
            switch(order)
            {
                case OrderType.Asc:
                    return await Categories.OrderBy(c => c.Title).ToListAsync<Category>();
                case OrderType.Desc:
                    return await Categories.OrderByDescending(c => c.Title).ToListAsync<Category>();
                default:
                    return await Categories.ToListAsync<Category>();
            }           
        }
        public Category GetCategoryById(int id)
        {
            return Categories.Where(c => c.Id == id).FirstOrDefault();
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await Categories.FirstOrDefaultAsync<Category>(c => c.Id == id);
        }
        public void AddCategory(Category category)
        {
            if (false == Categories.Contains(category))
            {
                Categories.Add(category);
            }
            else
            {
                throw new DuplicateWaitObjectException();
            }
        }
        public async Task AddCategoryAsync(Category category)
        {
            if(false == Categories.Contains(category))
            {
                await Categories.AddAsync(category);
            } 
            else
            {
                throw new DuplicateWaitObjectException();
            }
        }
        public void UpdateCategory(int id, Category category)
        {
            if (false == Categories.Contains(category))
            {
                Category categoryToEdit = Categories.Where(c => c.Id == id).FirstOrDefault();
                categoryToEdit = category;

                Categories.Update(categoryToEdit);
            }
            else
            {
                throw new DuplicateWaitObjectException();
            }
        }
        public async Task UpdateCategoryAsync(int id, Category category)
        {
            if (false == Categories.Contains(category))
            {
                Category categoryToEdit = await Categories.FirstOrDefaultAsync<Category>(c => c.Id == id);
                categoryToEdit = category;

                Categories.Update(categoryToEdit);
            }
            else
            {
                throw new DuplicateWaitObjectException();
            }
        }
        public void DeleteCategoryById(int id)
        {
            Category categoryToDelete = Categories.Where(c => c.Id == id).FirstOrDefault();
            Categories.Remove(categoryToDelete);
        }
        public async Task DeleteCategoryByIdAsync(int id)
        {
            Category categoryToDelete = await Categories.FirstOrDefaultAsync<Category>(c => c.Id == id);
            Categories.Remove(categoryToDelete);
        }
        public IEnumerable<Property> GetPropertiesByCategoryId(int id)
        {
            return Properties.Where(p => p.IdCategory.Equals(id)).ToList();
        }
        public async Task<IEnumerable<Property>> GetPropertiesByCategoryIdAsync(int id)
        {
            return await Properties.Where(p => p.IdCategory.Equals(id)).ToListAsync<Property>();
        }
        public IEnumerable<Property> GetProperties()
        {
            return Properties.ToList();
        }
        public async Task<IEnumerable<Property>> GetPropertiesAsync()
        {
            return await Properties.ToListAsync<Property>();
        }
        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await Properties.FirstOrDefaultAsync<Property>(c => c.Id == id);
        }
        public Property GetPropertyById(int id)
        {
            return Properties.Where(c => c.Id == id).FirstOrDefault();
        }
        #endregion
        #region Private methods
        private static void PopolateContext(DbContextOptions options)
        {
            using (var context = new ApplicationDbContext(options))
            {
                if (context.Categories.Any() || context.Properties.Any())
                {
                    context.Remove(context.Categories);
                    context.Remove(context.Properties);
                }

                context.Categories.AddRange(
                    new Category
                    {
                        Id = 1,
                        Title = "Apartment"
                    },
                    new Category
                    {
                        Id = 2,
                        Title = "Villa"
                    },
                    new Category
                    {
                        Id = 3,
                        Title = "Office"
                    });
                context.Properties.AddRange(
                    new Property
                    {
                        Id = 1,
                        IdCategory = 1,
                        Address = "7701 Woodlawn Ave",
                        Size = 88,
                        Value = 300000,
                        Year = new DateTime(2018, 11, 05)
                    },
                    new Property
                    {
                        Id = 2,
                        IdCategory = 1,
                        Address = "7272 Chestnut Ave",
                        Size = 300,
                        Value = 100000000,
                        Year = new DateTime(2018, 06, 17)
                    },
                    new Property
                    {
                        Id = 3,
                        IdCategory = 2,
                        Address = "8365 Fisher Rd",
                        Size = 1000,
                        Value = 250000,
                        Year = new DateTime(2016, 02, 22)
                    });

                context.SaveChanges();
            }
        }
        #endregion
    }
}
