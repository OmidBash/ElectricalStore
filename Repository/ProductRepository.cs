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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ElectricalStoreContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(PaginationFilter paginationFilter = null)
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
               .OrderBy(p => p.Title)
               .ToListAsync();
        }

        public async Task<int> GetCountProductAsync()
        {
            return await base.Count();
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await FindByCondition(product => product.Id.Equals(productId))
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductWithDetailsAsync(Guid productId)
        {
            return await FindByCondition(product => product.Id.Equals(productId))
                .Include(p => p.ProductImage)
                .Include(p => p.CategoryProduct).ThenInclude(cp => cp.Category)
                .Include(p => p.ProductFeature).ThenInclude(pf => pf.Feature)
                .ThenInclude(f => f.FeatureType)
                .FirstOrDefaultAsync();
        }

        public void CreateProduct(Product product)
        {
            base.Create(product);
        }

        public void UpdateProduct(Product product)
        {
            base.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            base.Delete(product);
        }
    }
}