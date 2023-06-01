using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MixedDreams.Infrastructure.Features.IngredientFeatures.GetIngredient;
using MixedDreams.Infrastructure.Features.ProductFeatures.ProductIngredient;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.ProductFeatures.PostPutProduct
{
    public sealed record PostProductRequest
    {
        [BindRequired]
        public string Name { get; init; }

        public string Description { get; init; } = string.Empty;

        public decimal? Price { get; init; }

        public int? AmountInStock { get; init; }

        public Visibility Visibility { get; init; } = Visibility.Unavaiable;

        public IFormFile PrimaryImage { get; init; }

        public float? RecommendedTemperature { get; init; }

        public float? RecommendedHumidity { get; init; }

        public IEnumerable<PostProductIngredientDto> Ingredients { get; init; }

        public Guid? ProductCategoryId { get; init; }
    }
}
