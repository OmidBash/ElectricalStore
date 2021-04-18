using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IRepositoryWrapper
    {
        ICategoryRepository Category { get; } 
        IProductRepository Product { get; } 
        IProductImageRepository ProductImage { get; } 
        IProductFeatureJuncRepository ProductFeatureJunc { get; }
        ICategoryProductJuncRepository CategoryProductJunc { get; }
        IFeatureTypeRepository FeatureType { get; }
        IFeatureRepository Feature { get; }

        Task SaveAsync();
    }
}