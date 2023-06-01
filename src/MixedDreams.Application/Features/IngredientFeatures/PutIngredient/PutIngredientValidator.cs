using FluentValidation;
using MixedDreams.Infrastructure.Features.IngredientFeatures.PostIngredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.IngredientFeatures.PutIngredient
{
    public class PutIngredientValidator : AbstractValidator<PutIngredientRequest>
    {
        public PutIngredientValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(50);
        }
    }
}
