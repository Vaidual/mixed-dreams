﻿using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class BusinessLocationRepository : BaseRepository<BusinessLocation>, IBusinessLocationRepository
    {
        private readonly AppDbContext _context;
        public BusinessLocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
