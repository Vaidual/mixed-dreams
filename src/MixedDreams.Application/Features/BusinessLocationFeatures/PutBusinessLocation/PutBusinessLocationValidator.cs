using FluentValidation;
using MixedDreams.Infrastructure.Features.Common.Address;
using MixedDreams.Infrastructure.Features.IngredientFeatures.PostIngredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.BusinessLocationFeatures.PutBusinessLocation
{
    public class PutBusinessLocationValidator : AbstractValidator<PutBusinessLocationRequest>
    {
        public PutBusinessLocationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Address).SetValidator(new AddressValidator());
        }
    }
}
