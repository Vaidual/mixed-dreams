using AutoMapper;
using MixedDreams.Infrastructure.Features.IngredientFeatures.GetIngredient;
using MixedDreams.Infrastructure.Features.IngredientFeatures.PostIngredient;
using MixedDreams.Infrastructure.Features.IngredientFeatures.PutIngredient;
using MixedDreams.Infrastructure.Features.ProductFeatures.PutProduct;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.IngredientFeatures
{
    public class IngredientMapper : Profile
    {
        public IngredientMapper()
        {
            CreateMap<PutIngredientRequest, Ingredient>();
            CreateMap<PostIngredientRequest, Ingredient>();
            CreateMap<Ingredient, GetIngredientResponse>();
        }
    }
}
