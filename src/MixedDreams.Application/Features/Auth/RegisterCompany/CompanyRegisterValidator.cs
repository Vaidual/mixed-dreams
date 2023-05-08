using FluentValidation;
using MixedDreams.Application.Features.Auth.Login;
using MixedDreams.Application.Features.Common;
using MixedDreams.Application.Features.Common.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Auth.RegisterCompany
{
    public class CompanyRegisterValidator : AbstractValidator<CompanyRegisterRequest>
    {
        public CompanyRegisterValidator()
        {
            Include(new RegisterValidator());
            RuleFor(x => x.Birthday).NotNull().LessThan(DateTime.Now.AddYears(16)).WithMessage("You must be at least 16 years old").GreaterThan(new DateTime(1800, 1, 1));
            RuleFor(x => x.CompanyName).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Address).SetValidator(new AddressValidator());
        }
    }
}
