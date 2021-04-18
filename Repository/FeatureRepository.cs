using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Entities.Filters;
using Contracts.Repositories;

namespace Repository
{
    public class FeatureRepository : RepositoryBase<Feature>, IFeatureRepository
    {
        public FeatureRepository(ElectricalStoreContext context) : base(context) { }

        public async Task<IEnumerable<Feature>> GetAllFeaturesAsync(PaginationFilter paginationFilter = null)
        {
            if (paginationFilter is not null)
            {
                return await FindAll()
                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize)
                    .OrderBy(cat => cat.Title)
                    .ToListAsync();
            }

            return await FindAll()
               .OrderBy(cat => cat.Title)
               .ToListAsync();
        }

        public async Task<int> GetCountFeatureAsync()
        {
            return await base.Count();
        }

        public async Task<Feature> GetFeatureByIdAsync(Guid featureId)
        {
            return await FindByCondition(feature => feature.Id.Equals(featureId))
                .FirstOrDefaultAsync();
        }

        public async Task<Feature> GetFeatureWithDetailsAsync(Guid featureId)
        {
            return await FindByCondition(feature => feature.Id.Equals(featureId))
                .Include(ft => ft.FeatureType)
                .Include(feature => feature.ProductFeature).ThenInclude(pf => pf.Product)
                .FirstOrDefaultAsync();
        }

        public void CreateFeature(Feature feature)
        {
            base.Create(feature);
        }

        public void UpdateFeature(Feature feature)
        {
            base.Update(feature);
        }

        public void DeleteFeature(Feature feature)
        {
            base.Delete(feature);
        }

        public void DeleteFeatureRange(List<Feature> features)
        {
            base.DeleteRange(features);
        }

        public void DetachFeature(List<Feature> features)
        {
            base.DetachEntity(features);
        }
    }
}