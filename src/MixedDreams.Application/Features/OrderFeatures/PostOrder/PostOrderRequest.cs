using Microsoft.AspNetCore.Mvc.ModelBinding;
using MixedDreams.Application.Features.OrderFeatures.OrderProduct;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.OrderFeatures.PostOrder
{
    public sealed record PostOrderRequest
    {
        [BindRequired]
        public Guid BusinessLocationId { get; set; }

        [BindRequired]
        public List<PostOrderProductDto> Products { get; set; }
    }
}
