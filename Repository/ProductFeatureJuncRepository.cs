using System.Collections.Generic;
using Contracts.Repositories;
using Entities;
using Entities.Models;
using Repository;

namespace Repository
{
    public class ProductFeatureJuncRepository : RepositoryBase<ProductFeatureJunction>, IProductFeatureJuncRepository
    {
        public ProductFeatureJuncRepository(ElectricalStoreContext context) : base(context) { }

        public void DeleteProductFeatureJuncRange(List<ProductFeatureJunction> ProductFeatures)
        {
            DeleteRange(ProductFeatures);
        }
    }
}