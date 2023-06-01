using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        public Task<Guid?> GetCompanyIdByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        public string? GettenantIdByBusinessLocationIdAsync(Guid locationId, CancellationToken cancellationToken = default);
    }
}
