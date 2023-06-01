using AutoMapper;
using MixedDreams.Infrastructure.Features.OrderFeatures.GetOrder;
using MixedDreams.Infrastructure.Features.OrderFeatures.PostOrder;
using MixedDreams.Infrastructure.Features.OrderFeatures.UpdateOrderStatus;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.OrderFeatures
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<Order, GetOrderResponse>()
                .ForMember(
                    dest => dest.Address, 
                    (opt) => opt.MapFrom(src => src.BusinessLocation.Address));

            CreateMap<PostOrderRequest, Order>();
            CreateMap<UpdateOrderStatusRequest, Order>()
                .ForMember(
                dest => dest.OrderStatus, 
                opt => opt.MapFrom(src => src.Status));
        }
    }
}
