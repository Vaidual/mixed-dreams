using MixedDreams.Application.Features.ProductFeatures.Dto;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.ProductFeatures.GetProduct
{
    public sealed class GetProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? PrimaryImage { get; set; }
        public string Category { get; set; }
        public CompanyDto Company { get; set; }
        public IEnumerable<IngredientDto> Ingredients { get; set; }
    }
}
