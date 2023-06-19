using MixedDreams.Application.Features.OrderFeatures.PostOrder;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Hubs.Clients
{
    public interface IOrderService
    {
        public Task<Order> PlaceOrderAsync(PostOrderRequest model, ClaimsPrincipal user);
    }
}
