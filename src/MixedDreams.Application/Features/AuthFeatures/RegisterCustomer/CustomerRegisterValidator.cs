using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.AuthFeatures.RegisterCustomer
{
    public class CustomerRegisterValidator : AbstractValidator<CustomerRegisterRequest>
    {
        public CustomerRegisterValidator()
        {
            Include(new RegisterValidator());
        }
    }
}
