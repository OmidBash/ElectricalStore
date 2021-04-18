using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Filters;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IFeatureTypeRepository
    {
        Task<IEnumerable<FeatureType>> GetAllFeatureTypesAsync(PaginationFilter paginationFilter = null);
        Task<int> GetCountFeatureTypeAsync();
        Task<FeatureType> GetFeatureTypeByIdAsync(Guid featureTypeId);
        Task<FeatureType> GetFeatureTypeWithDetailsAsync(Guid featureTypeId);
        void CreateFeatureType(FeatureType featureType);
        void UpdateFeatureType(FeatureType featureType);
        void DeleteFeatureType(FeatureType featureType);
        void DetachFeatureType(List<FeatureType> featureTypes);
    }
}