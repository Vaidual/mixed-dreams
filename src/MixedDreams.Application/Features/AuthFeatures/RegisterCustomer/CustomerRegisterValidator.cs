using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.AuthFeatures.RegisterCustomer
{
    public class CustomerRegisterValidator : AbstractValidator<CustomerRegisterRequest>
    {
        public CustomerRegisterValidator()
        {
            Include(new RegisterValidator());
        }
    }
}
