using FluentValidation;
using MixedDreams.Infrastructure.Features.Common.Address;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.BusinessLocationFeatures.PostBusinessLocation
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
