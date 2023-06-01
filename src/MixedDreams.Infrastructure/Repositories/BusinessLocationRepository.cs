using MixedDreams.Infrastructure.RepositoryInterfaces;
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
        public BusinessLocationRepository(AppDbContext context) : base(context) { }

        public Task<bool> IsNameTaken(string name)
        {
            return ExistAnyAsync(x => x.Name == name);
        }
    }
}
