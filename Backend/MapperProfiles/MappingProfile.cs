using AutoMapper;
using MixedDreams.Core.ViewModels;
using MixedDreams.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.WebAPI.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
