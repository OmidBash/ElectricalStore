using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Filters;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(PaginationFilter paginationFilter = null);
        Task<int> GetCountProductAsync();
        Task<Product> GetProductByIdAsync(Guid productId);
        Task<Product> GetProductWithDetailsAsync(Guid productId);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}