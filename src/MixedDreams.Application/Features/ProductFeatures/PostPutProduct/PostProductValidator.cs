using FluentValidation;
using MixedDreams.Infrastructure.Extensions;
using MixedDreams.Infrastructure.Features.ProductFeatures.ProductIngredient;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.ProductFeatures.PostPutProduct
{
    public class PostProductValidator : AbstractValidator<PostProductRequest>
    {
        public PostProductValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AmountInStock)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Description)
                .MaximumLength(4000);
            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(50);
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);
            //RuleFor(x => x.PrimaryImage).;
            RuleFor(x => x.RecommendedHumidity)
                .InclusiveBetween(0, 100);
            RuleFor(x => x.RecommendedTemperature)
                .InclusiveBetween(-89.2f, 500);
            RuleFor(x => x.Visibility).NotEmpty()
                .IsInEnum();
            When(x => x.Ingredients != null, () =>
            {
                RuleForEach(x => x.Ingredients).SetValidator(new ProductIngredientDtoValidator());
            });
            When(x => x.PrimaryImage != null, () =>
            {
                RuleFor(x => x.PrimaryImage).Image();
            });
            When(x => x.ProductCategoryId != null, () =>
            {
                RuleFor(x => x.ProductCategoryId).Cascade(CascadeMode.Stop)
                .MustAsync((x, token) => unitOfWork.ProductCategoryRepository.EntityExistsAsync((Guid)x!)).WithMessage("Product category with id '{PropertyValue}' doen't exist.");
            });
            
        }
    }
}
