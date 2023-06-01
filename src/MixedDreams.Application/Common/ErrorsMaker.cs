﻿using FluentValidation.Results;
using MixedDreams.Infrastructure.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace MixedDreams.Infrastructure.Common
{
    public class ErrorsMaker
    {
        public static void ProcessValidationErrors(List<ValidationFailure> failures)
        {
            var errors = failures
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage)
                );
            throw new ModelValidationException(errors);
        }
    }
}
