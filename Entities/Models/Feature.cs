using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class Feature
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid FeatureTypeId { get; set; }
        
        public FeatureType FeatureType { get; set; }
        
        public IEnumerable<ProductFeatureJunction> ProductFeature { get; set; }
    }
}