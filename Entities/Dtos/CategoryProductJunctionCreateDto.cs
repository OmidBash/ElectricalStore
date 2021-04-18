using System;

namespace Entities.Dtos
{
    public class CategoryProductJunctionCreateDto
    {
        public Guid CategoryId { get; set; }
        public Guid ProductId { get; set; }
    }
}