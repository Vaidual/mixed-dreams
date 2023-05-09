using AutoMapper;
using MixedDreams.Application.Features.AuthFeatures;
using MixedDreams.Application.Features.AuthFeatures.RegisterCompany;
using MixedDreams.Application.Features.AuthFeatures.RegisterCustomer;
using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.AuthFeatures
{
    internal class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<CustomerRegisterRequest, Customer>();
            CreateMap<AddressDto, Address>();
            CreateMap<CompanyRegisterRequest, Company>();
        }
    }
}
