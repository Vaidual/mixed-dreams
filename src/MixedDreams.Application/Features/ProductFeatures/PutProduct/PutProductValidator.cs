using FluentValidation;
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
            RuleFor(x => x.AmountInStock).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.CompanyId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(x => x.PrimaryImage).MaximumLength(2100);
            RuleFor(x => x.ProductCategoryId).NotEmpty();
            RuleFor(x => x.RecommendedHumidity).NotNull().InclusiveBetween(0, 100);
            RuleFor(x => x.RecommendedTemperature).NotNull().InclusiveBetween(-89.2f, 500);
            RuleFor(x => x.Visibility).NotEmpty().IsInEnum();

            ClassLevelCascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.ProductCategoryId).Must(unitOfWork.ProductCategoryRepository.EntityExists).WithMessage("Product category with id '{PropertyValue}' doen't exist.");
            RuleFor(x => x.CompanyId).Must(unitOfWork.CompanyRepository.EntityExists).WithMessage("Company with id '{PropertyValue}' doen't exist.");
        }
    }
}
