using System;
using System.Collections.Generic;

namespace Entities.Dtos
{
    public class ProductFeatureJunctionReadDto
    {
        public Guid ProductId { get; set; }
        public Guid FeatureId { get; set; }
        public FeatureReadDto Feature { get; set; }
        public ProductReadDto Product { get; set; }
        public float Price { get; set; }
    }
}