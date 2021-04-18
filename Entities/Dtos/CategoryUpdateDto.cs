using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class CategoryUpdateDto
    {   
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }
    }
}