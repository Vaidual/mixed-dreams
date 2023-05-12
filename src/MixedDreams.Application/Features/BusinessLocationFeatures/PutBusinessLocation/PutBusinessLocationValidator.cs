using FluentValidation;
using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Application.Features.IngredientFeatures.PostIngredient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.BusinessLocationFeatures.PutBusinessLocation
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
