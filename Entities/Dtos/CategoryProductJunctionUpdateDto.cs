using System;

namespace Entities.Dtos
{
    public class CategoryProductJunctionUpdateDto
    {
        public Guid CategoryId { get; set; }
        public Guid ProductId { get; set; }
    }
}