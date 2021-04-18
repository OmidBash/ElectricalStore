using System;
using System.Collections.Generic;

namespace Entities.Dtos
{
    public class CategoryProductJunctionReadDto
    {
        public Guid CategoryId { get; set; }
        public Guid ProductId { get; set; }
        public CategoryReadDto Category { get; set; }
        public ProductReadDto Product { get; set; }
        
    }
}