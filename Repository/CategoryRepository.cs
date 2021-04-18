using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repositories;
using Entities;
using Entities.Filters;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ElectricalStoreContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(PaginationFilter paginationFilter = null)
        {
            if (paginationFilter is not null)
            {
                return await FindAll()
                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize)
                    .OrderBy(cat => cat.Title)
                    .ToListAsync();
            }

            return await FindAll()
               .OrderBy(cat => cat.Title)
               .ToListAsync();
        }

        public async Task<int> GetCountCategoryAsync()
        {
            return await base.Count();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await FindByCondition(category => category.Id.Equals(categoryId))
                .FirstOrDefaultAsync();
        }

        public async Task<Category> GetCategoryWithDetailsAsync(Guid categoryId)
        {
            return await FindByCondition(category => category.Id.Equals(categoryId))
                .Include(c => c.CategoryProduct).ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync();
        }

        public void CreateCategory(Category category)
        {
            base.Create(category);
        }

        public void UpdateCategory(Category category)
        {
            base.Update(category);
        }

        public void DeleteCategory(Category category)
        {
            base.Delete(category);
        }
    }
}