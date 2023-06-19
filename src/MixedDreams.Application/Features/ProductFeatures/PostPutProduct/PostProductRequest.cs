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
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.ModelBinders;

namespace MixedDreams.Application.Features.ProductFeatures.PostPutProduct
{
    public sealed record PostProductRequest
    {
        [BindRequired]
        public string Name { get; init; }

        [ModelBinder(typeof(EmptyStringToEmptyValueBinder))]
        public string Description { get; init; }

        public decimal? Price { get; init; }

        public int? AmountInStock { get; init; }

        public Visibility Visibility { get; init; } = Visibility.Unavaiable;

        public IFormFile PrimaryImage { get; init; }

        public float? RecommendedTemperature { get; init; }

        public short? PreparationTime { get; set; }

        public float? RecommendedHumidity { get; init; }

        public IEnumerable<PostProductIngredientDto> Ingredients { get; init; }

        public Guid? ProductCategoryId { get; init; }
    }
}
