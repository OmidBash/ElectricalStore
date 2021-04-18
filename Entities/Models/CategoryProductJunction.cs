
using System;

namespace Entities.Models
{
    public class CategoryProductJunction
    {      
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}