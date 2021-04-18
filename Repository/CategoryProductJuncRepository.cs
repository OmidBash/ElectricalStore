using System.Collections.Generic;
using Contracts.Repositories;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CategoryProductJuncRepository : RepositoryBase<CategoryProductJunction>, ICategoryProductJuncRepository
    {
        public CategoryProductJuncRepository(ElectricalStoreContext context) : base(context) { }
        
        public void DeleteCategoryProductJuncRange(List<CategoryProductJunction> categoryProducts)
        {
            DeleteRange(categoryProducts);
        }
    }
}