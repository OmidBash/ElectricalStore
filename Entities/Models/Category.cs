using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<CategoryProductJunction> CategoryProduct { get; set; }
    }
}