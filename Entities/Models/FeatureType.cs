using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class FeatureType
    {
        public Guid Id { get; set; }
        public string Title { get; set; }       
        public string Description { get; set; } 
        public IEnumerable<Feature> Features { get; set; }
    }
}