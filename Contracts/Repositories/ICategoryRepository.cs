using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Filters;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(PaginationFilter paginationFilter = null);
        Task<int> GetCountCategoryAsync();
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<Category> GetCategoryWithDetailsAsync(Guid categoryId);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}