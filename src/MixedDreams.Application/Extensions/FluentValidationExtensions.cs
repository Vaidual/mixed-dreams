using FluentValidation;
using Microsoft.AspNetCore.Http;
using MixedDreams.Infrastructure.Constants;
using MixedDreams.Infrastructure.Features.ProductFeatures.ProductIngredient;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.\(\)\{\}\[\]\-\=\+\&\^\%\$\#\@\:\;\`\~\|\\\'\""\/\,\>\<_]+").WithMessage("Your password must contain at least one special character");
        }

        public static IRuleBuilderOptions<T, IFormFile> Image<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x.Length <= ImageRequierements.maxSizeInBytes).WithMessage($"Maximum image upload size is {(double)ImageRequierements.maxSizeInBytes / 1_000_000} megabytes.")
                .Must(x => x.IsImageAsync()).WithMessage("Provided file is not a supported image file.");
        }
    }
}
