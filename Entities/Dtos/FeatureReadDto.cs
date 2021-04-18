using System;

namespace Entities.Dtos
{
    public class FeatureReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public FeatureTypeReadDto FeatureType { get; set; }
    }
}