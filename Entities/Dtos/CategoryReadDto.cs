using System;
using System.Collections.Generic;

namespace Entities.Dtos
{
    public class CategoryReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // public List<ProductReadDto> Products { get; set; }
    }
}