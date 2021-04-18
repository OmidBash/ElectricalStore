using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repositories;
using Entities;
using Entities.Filters;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class FeatureTypeRepository : RepositoryBase<FeatureType>, IFeatureTypeRepository
    {
        public FeatureTypeRepository(ElectricalStoreContext context) : base(context) { }

        public async Task<IEnumerable<FeatureType>> GetAllFeatureTypesAsync(PaginationFilter paginationFilter = null)
        {
            if (paginationFilter is not null)
            {
                return await FindAll()
                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize)
                    .OrderBy(ft => ft.Title)
                    .ToListAsync();
            }

            return await FindAll()
               .OrderBy(ft => ft.Title)
               .ToListAsync();
        }

        public async Task<int> GetCountFeatureTypeAsync()
        {
            return await base.Count();
        }

        public async Task<FeatureType> GetFeatureTypeByIdAsync(Guid featureTypeId)
        {
            return await FindByCondition(ft => ft.Id.Equals(featureTypeId))
                .FirstOrDefaultAsync();
        }

        public async Task<FeatureType> GetFeatureTypeWithDetailsAsync(Guid featureTypeId)
        {
            return await FindByCondition(ft => ft.Id.Equals(featureTypeId))
                .Include(ft => ft.Features)
                .FirstOrDefaultAsync();
        }

        public void CreateFeatureType(FeatureType featureType)
        {
            base.Create(featureType);
        }

        public void UpdateFeatureType(FeatureType featureType)
        {
            base.Update(featureType);
        }

        public void DeleteFeatureType(FeatureType featureType)
        {
            base.Delete(featureType);
        }
        
        public void DetachFeatureType(List<FeatureType> featureTypes)
        {
            base.DetachEntity(featureTypes);
        }
    }
}