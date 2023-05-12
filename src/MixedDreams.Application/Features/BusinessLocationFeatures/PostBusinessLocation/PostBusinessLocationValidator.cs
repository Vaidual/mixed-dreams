using FluentValidation;
using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Application.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.BusinessLocationFeatures.PostBusinessLocation
{
    public class PostBusinessLocationValidator : AbstractValidator<PostBusinessLocationRequest>
    {
        public PostBusinessLocationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Address).SetValidator(new AddressValidator());
        }
    }
}
