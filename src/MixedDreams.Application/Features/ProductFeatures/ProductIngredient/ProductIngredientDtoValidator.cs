using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.ProductIngredient
{
    public class ProductIngredientDtoValidator : AbstractValidator<PostProductIngredientDto>
    {
        public ProductIngredientDtoValidator()
        {
            When(x => x.HasAmount, () =>
            {
                RuleFor(x => x.Amount).NotNull()
                    .GreaterThan(0)
                    .LessThanOrEqualTo(float.MaxValue);
                RuleFor(x => x.Unit).NotNull().IsInEnum();
            });
        }
    }
}
