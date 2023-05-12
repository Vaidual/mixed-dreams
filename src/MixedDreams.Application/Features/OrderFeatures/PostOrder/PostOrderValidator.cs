using FluentValidation;
using Microsoft.AspNetCore.Http.Connections;
using MixedDreams.Application.Features.OrderFeatures.OrderProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.OrderFeatures.PostOrder
{
    public class PostOrderValidator : AbstractValidator<PostOrderRequest>
    {
        public PostOrderValidator()
        {
            RuleFor(x => x.BusinessLocationId).NotNull();
            RuleFor(x => x.Products).NotNull()
                .Must(x => x.Count > 0).WithMessage("Order must have at least one product.");
            RuleForEach(x => x.Products).SetValidator(new PostOrderProductDtoValidator());
        }
    }
}
