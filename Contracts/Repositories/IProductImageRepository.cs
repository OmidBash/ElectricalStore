using System.Collections.Generic;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IProductImageRepository
    {
        void DeleteImageProductRange(List<ProductImage> productImages);
    }
}