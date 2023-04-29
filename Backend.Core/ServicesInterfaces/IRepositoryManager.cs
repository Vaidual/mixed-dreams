using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MixedDreams.Core.RepositoryInterfaces;
using MixedDreams.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Core.ServicesInterfaces
{
    public interface IRepositoryManager
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
