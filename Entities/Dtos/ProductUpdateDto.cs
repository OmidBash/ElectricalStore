using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class ProductUpdateDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }
        [Required]
        public float Price { get; set; }
        public List<CategoryProductJunctionUpdateDto> Categories { get; set; }
        public List<ProductImageUpdateDto> ImagePaths { get; set; }
        public List<ProductFeatureJunctionUpdateDto> Features { get; set; }
    }
}