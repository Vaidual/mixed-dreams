using Microsoft.AspNetCore.Mvc.ModelBinding;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.OrderFeatures.UpdateOrderStatus
{
    public class UpdateOrderStatusRequest
    {
        [BindRequired]
        public OrderStatus Status { get; set; }
    }
}
