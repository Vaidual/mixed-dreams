using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.GetProductWithDetails
{
    public class ProductDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public string? PrimaryImage { get; set; }
        public int? AmountInStock { get; set; }
        public Visibility Visibility { get; set; }
        public float? RecommendedTemperature { get; set; }
        public float? RecommendedHumidity { get; set; }
        public IEnumerable<GetProductIngredientsDto> Ingredients { get; set; }
    }
}
