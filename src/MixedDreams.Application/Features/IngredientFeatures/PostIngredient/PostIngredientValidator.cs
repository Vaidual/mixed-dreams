using FluentValidation;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.IngredientFeatures.PostIngredient
{
    public class PostIngredientValidator : AbstractValidator<PostIngredientRequest>
    {
        public PostIngredientValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(50);
        }
    }
}
