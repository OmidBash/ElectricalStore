using System;

namespace Entities.Dtos
{
    public class ProductImageUpdateDto
    {
        public Guid Id { get; set; }
        public string URL { get; set; }
        public string Caption { get; set; }
    }
}