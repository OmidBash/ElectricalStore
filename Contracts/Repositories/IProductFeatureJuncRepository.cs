using System.Collections.Generic;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IProductFeatureJuncRepository
    {
        void DeleteProductFeatureJuncRange(List<ProductFeatureJunction> productFeatures);
    }
}