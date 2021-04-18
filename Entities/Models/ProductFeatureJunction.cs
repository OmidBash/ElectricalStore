using System;

namespace Entities.Models
{
    public class ProductFeatureJunction
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
                
        public Guid FeatureId { get; set; }
        public Feature Feature { get; set; }

        public float Price { get; set; }
    }
}