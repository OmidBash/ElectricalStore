using System;

namespace Entities.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string URL { get; set; }
        public string Caption { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}