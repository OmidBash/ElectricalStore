using System.Collections.Generic;

namespace Entities.Dtos
{
    public class ProductCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public List<CategoryProductJunctionCreateDto> Categories { get; set; }
        public List<ProductImageCreateDto> ImagePaths { get; set; }
        public List<ProductFeatureJunctionCreateDto> Features { get; set; }        
    }
}