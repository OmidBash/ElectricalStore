using System;
using System.Collections.Generic;

namespace Entities.Dtos
{
    public class FeatureTypeReadDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<FeatureReadDto> Features { get; set; }
    }
}