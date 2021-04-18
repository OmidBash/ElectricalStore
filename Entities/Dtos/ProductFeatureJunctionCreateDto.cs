using System;

namespace Entities.Dtos
{
    public class ProductFeatureJunctionCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid FeatureId { get; set; }
        public float Price { get; set; }
    }
}