using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Common.Address
{
    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.ZipCode).NotEmpty().Matches("^[0-9]{5}(?:-[0-9]{4})?$");
            RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Street).NotEmpty().MaximumLength(100);
            RuleFor(x => x.State).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Apartment).MaximumLength(100);
        }
    }
}
