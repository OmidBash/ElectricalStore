using System;

namespace Entities.Dtos
{
    public class ProductFeatureJunctionUpdateDto
    {
        public Guid ProductId { get; set; }
        public Guid FeatureId { get; set; }
        public float Price { get; set; }
    }
}