using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.IngredientFeatures.PostIngredient
{
    public sealed record PostIngredientRequest
    {
        public string Name { get; init; }
    }
}
