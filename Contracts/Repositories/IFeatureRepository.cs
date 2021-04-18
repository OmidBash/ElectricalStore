using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Filters;
using Entities.Models;

namespace Contracts.Repositories
{
    public interface IFeatureRepository : IRepositoryBase<Feature>
    {
        Task<IEnumerable<Feature>> GetAllFeaturesAsync(PaginationFilter paginationFilter = null);
        Task<int> GetCountFeatureAsync();
        Task<Feature> GetFeatureByIdAsync(Guid featureId);
        Task<Feature> GetFeatureWithDetailsAsync(Guid featureId);
        void CreateFeature(Feature feature);
        void UpdateFeature(Feature feature);
        void DeleteFeature(Feature feature);
        void DeleteFeatureRange(List<Feature> features);
        void DetachFeature(List<Feature> features);
    }
}