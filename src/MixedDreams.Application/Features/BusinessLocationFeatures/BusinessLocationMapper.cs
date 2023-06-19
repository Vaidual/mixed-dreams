using AutoMapper;
using MixedDreams.Application.Features.BusinessLocationFeatures.GetBusinessLocation;
using MixedDreams.Application.Features.BusinessLocationFeatures.PostBusinessLocation;
using MixedDreams.Application.Features.BusinessLocationFeatures.PutBusinessLocation;
using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.BusinessLocationFeatures
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
