using AutoMapper;
using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features
{
    public class CommonMapper : Profile
    {
        public CommonMapper()
        {
            CreateMap<AddressDto, Address>().ReverseMap();
        }
    }
}
