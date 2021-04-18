using System.Collections.Generic;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface ICategoryProductJuncRepository
    {
         void DeleteCategoryProductJuncRange(List<CategoryProductJunction> categoryProducts);
    }
}