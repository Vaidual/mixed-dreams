using AutoMapper;
using MixedDreams.Infrastructure.Features.AuthFeatures;
using MixedDreams.Infrastructure.Features.AuthFeatures.RegisterCompany;
using MixedDreams.Infrastructure.Features.AuthFeatures.RegisterCustomer;
using MixedDreams.Infrastructure.Features.Common.Address;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.AuthFeatures
{
    internal class AuthMapper : Profile
    {
        public AuthMapper()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<CustomerRegisterRequest, Customer>();
            CreateMap<CompanyRegisterRequest, Company>();
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
