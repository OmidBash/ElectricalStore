using System;
using System.Collections.Generic;

namespace Entities.Dtos
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public List<CategoryProductJunctionReadDto> Categories { get; set; }
        public List<ProductImageReadDto> ImagePaths { get; set; }
        public List<ProductFeatureJunctionReadDto> Features { get; set; }
    }
}