using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class FeatureTypeCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public List<FeatureCreateDto> Features { get; set; }
    }
}