using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class FeatureUpdateDto
    {
        [Required]
        public Guid Id { get; set; }
        public Guid FeatureTypeId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }
    }
}