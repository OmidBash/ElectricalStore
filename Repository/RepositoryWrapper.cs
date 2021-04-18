using System.Threading.Tasks;
using Contracts.Repositories;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ElectricalStoreContext _repoContext;
        private CategoryRepository _category;
        private ProductRepository _product;
        private ProductImageRepository _productImage;
        private FeatureTypeRepository _featureType;
        private FeatureRepository _feature;
        private ProductFeatureJuncRepository _productFeatureJunc;
        private CategoryProductJuncRepository _CategoryProductJunc;

        public ICategoryRepository Category
        {
            get
            {
                if (_category is null)
                    _category = new CategoryRepository(_repoContext);

                return _category;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product is null)
                    _product = new ProductRepository(_repoContext);

                return _product;
            }
        }

        public IProductImageRepository ProductImage
        {
            get
            {
                if (_productImage is null)
                    _productImage = new ProductImageRepository(_repoContext);

                return _productImage;
            }
        }

        public IFeatureTypeRepository FeatureType
        {
            get
            {
                if (_featureType is null)
                    _featureType = new FeatureTypeRepository(_repoContext);

                return _featureType;
            }
        }

        public IFeatureRepository Feature
        {
            get
            {
                if (_feature is null)
                    _feature = new FeatureRepository(_repoContext);

                return _feature;
            }
        }

        public IProductFeatureJuncRepository ProductFeatureJunc
        {
            get
            {
                if (_productFeatureJunc is null)
                    _productFeatureJunc = new ProductFeatureJuncRepository(_repoContext);

                return _productFeatureJunc;
            }
        }

        public ICategoryProductJuncRepository CategoryProductJunc
        {
            get
            {
                if (_CategoryProductJunc is null)
                    _CategoryProductJunc = new CategoryProductJuncRepository(_repoContext);

                return _CategoryProductJunc;
            }
        }

        public RepositoryWrapper(ElectricalStoreContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}