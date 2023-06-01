using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.OrderFeatures.OrderProduct
{
    public class PostOrderProductDtoValidator : AbstractValidator<PostOrderProductDto>
    {
        public PostOrderProductDtoValidator()
        {
            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.Amount).NotNull()
                .GreaterThan(0).WithMessage("You cant order less then one product.");
        }
    }
}
