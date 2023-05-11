using FluentValidation;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
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

            RuleFor(x => x.RecommendedHumidity).NotNull()
                .InclusiveBetween(0, 100);
            RuleFor(x => x.RecommendedTemperature).NotNull()
                .InclusiveBetween(-89.2f, 500);
            RuleFor(x => x.Visibility).NotEmpty()
                .IsInEnum();

            RuleFor(x => x.ProductCategoryId).Cascade(CascadeMode.Stop)
                .NotNull()
                .MustAsync((x, token) => unitOfWork.ProductCategoryRepository.EntityExists((Guid)x!)).WithMessage("Product category with id '{PropertyValue}' doen't exist.");
            RuleFor(x => x.CompanyId).Cascade(CascadeMode.Stop)
                .NotNull()
                .MustAsync((x, token) => unitOfWork.CompanyRepository.EntityExists((Guid)x!)).WithMessage("Company with id '{PropertyValue}' doen't exist.");
        }
    }
}
