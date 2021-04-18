using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public IEnumerable<CategoryProductJunction> CategoryProduct { get; set; }
        public IEnumerable<ProductImage> ProductImage { get; set; }
        public IEnumerable<ProductFeatureJunction> ProductFeature { get; set; }
    }
}