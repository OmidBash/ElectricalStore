using System.Collections.Generic;
using Contracts.Repositories;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ProductImageRepository : RepositoryBase<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ElectricalStoreContext context) : base(context) { }

        public void DeleteImageProductRange(List<ProductImage> productImages)
        {
            base.DeleteRange(productImages);
        }
    }
}