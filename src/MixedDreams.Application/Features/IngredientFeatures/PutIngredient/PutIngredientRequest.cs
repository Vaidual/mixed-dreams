using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.IngredientFeatures.PutIngredient
{
    public sealed record PutIngredientRequest
    {
        public string Name { get; init; }
    }
}
