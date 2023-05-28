using Microsoft.AspNetCore.Mvc.ModelBinding;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.GetProductWithDetails
{
    public record GetProductIngredientsDto
    {
        [BindRequired]
        public Guid Id { get; init; }

        [BindRequired]
        public string Name { get; init; }

        [BindRequired]
        public bool HasAmount { get; init; }

        public float? Amount { get; init; }
        public Unit? Unit { get; init; }
    }
}
