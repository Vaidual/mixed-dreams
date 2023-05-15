using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context) { }

        public async Task<Guid?> GetCompanyIdByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return (await Table.FirstOrDefaultAsync(x => x.ApplicationUserId == userId, cancellationToken))?.Id;
        }
        public string? GettenantIdByBusinessLocationIdAsync(Guid locationId, CancellationToken cancellationToken = default)
        {
            return Context.BusinessLocations.Include(x => x.Company).FirstOrDefault(x => x.Id == locationId)?.Company.ApplicationUserId;
        }
    }
}
