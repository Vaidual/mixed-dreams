using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MixedDreams.Application.Features.IngredientFeatures.GetIngredient;
using MixedDreams.Application.Features.ProductFeatures.ProductIngredient;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.PostPutProduct
{
    public sealed record PostProductRequest
    {
        [BindRequired]
        public string Name { get; init; }

        public string Description { get; init; } = string.Empty;

        [BindRequired]
        public decimal Price { get; init; }

        [BindRequired]
        public int AmountInStock { get; init; }

        public Visibility Visibility { get; init; } = Visibility.Unavaiable;

        public IFormFile? PrimaryImage { get; init; }

        [BindRequired]
        public float RecommendedTemperature { get; init; }

        [BindRequired]
        public float RecommendedHumidity { get; init; }

        public IEnumerable<ProductIngredientDto>? Ingredients { get; init; }

        [BindRequired]
        public Guid ProductCategoryId { get; init; }
    }
}
