using AutoMapper;
using MixedDreams.Application.Features.Auth;
using MixedDreams.Application.Features.Auth.RegisterCompany;
using MixedDreams.Application.Features.Auth.RegisterCustomer;
using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.MapperProfiles
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<CustomerRegisterRequest, Customer>();
            CreateMap<AddressDto, Address>();
            CreateMap<CompanyRegisterRequest, Company>();
        }
    }
}
