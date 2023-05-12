using AutoMapper;
using MixedDreams.Application.Features.IngredientFeatures.GetIngredient;
using MixedDreams.Application.Features.IngredientFeatures.PostIngredient;
using MixedDreams.Application.Features.IngredientFeatures.PutIngredient;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.IngredientFeatures
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
