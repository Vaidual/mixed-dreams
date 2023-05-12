using FluentValidation;
using MixedDreams.Application.Extensions;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.Features.ProductFeatures.ProductIngredient;
using MixedDreams.Application.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.PutProduct
{
    public class PutProductValidator : AbstractValidator<PutProductRequest>
    {
        public PutProductValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AmountInStock).NotNull()
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Description).NotNull()
                .MaximumLength(4000);
            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.Price).NotNull()
                .GreaterThanOrEqualTo(0);
            When(x => x.PrimaryImage != null, () =>
            {
                RuleFor(x => x.PrimaryImage).Image();
            });
            RuleFor(x => x.RecommendedHumidity).NotNull()
                .InclusiveBetween(0, 100);
            RuleFor(x => x.RecommendedTemperature).NotNull()
                .InclusiveBetween(-89.2f, 500);
            RuleFor(x => x.Visibility).NotEmpty()
                .IsInEnum();

            RuleFor(x => x.Visibility).NotEmpty()
                .IsInEnum();
            When(x => x.Ingredients != null, () =>
            {
                RuleForEach(x => x.Ingredients).SetValidator(new ProductIngredientDtoValidator());
            });
            RuleFor(x => x.ProductCategoryId).Cascade(CascadeMode.Stop)
                .NotNull()
                .MustAsync((x, token) => unitOfWork.ProductCategoryRepository.EntityExistsAsync((Guid)x!)).WithMessage("Product category with id '{PropertyValue}' doen't exist.");
        }
    }
}
