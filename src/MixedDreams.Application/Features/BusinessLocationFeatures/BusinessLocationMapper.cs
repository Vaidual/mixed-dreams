using AutoMapper;
using MixedDreams.Infrastructure.Features.BusinessLocationFeatures.GetBusinessLocation;
using MixedDreams.Infrastructure.Features.BusinessLocationFeatures.PostBusinessLocation;
using MixedDreams.Infrastructure.Features.BusinessLocationFeatures.PutBusinessLocation;
using MixedDreams.Infrastructure.Features.Common.Address;
using MixedDreams.Infrastructure.Features.ProductFeatures.PutProduct;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.BusinessLocationFeatures
{
    public class BusinessLocationMapper : Profile
    {
        public BusinessLocationMapper()
        {
            CreateMap<PutBusinessLocationRequest, BusinessLocation>();
            CreateMap<PostBusinessLocationRequest, BusinessLocation>();
            CreateMap<BusinessLocation, GetBusinessLocationResponse>();
        }
    }
}
