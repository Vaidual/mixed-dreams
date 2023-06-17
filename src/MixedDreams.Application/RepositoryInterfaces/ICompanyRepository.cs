using MixedDreams.Application.Features.CompanyFeatures.GetSettings;
using MixedDreams.Application.Features.CompanyFeatures.PutCompanySettings;
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
        public Task<GetCompanySettings>? GetSettings(Guid companyId, CancellationToken cancellationToken = default);
        public Task UpdateSettings(Guid companyId, PutCompanySettingsRequest newSettings);
    }
}
