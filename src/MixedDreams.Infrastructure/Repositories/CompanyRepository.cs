using Microsoft.EntityFrameworkCore;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MixedDreams.Application.Features.CompanyFeatures.GetSettings;
using MixedDreams.Infrastructure.Exceptions.NotFound;
using MixedDreams.Application.Features.CompanyFeatures.PutCompanySettings;

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

        public async Task<GetCompanySettings>? GetSettings(Guid companyId, CancellationToken cancellationToken = default)
        {
            var company = await this.GetAsync(companyId, cancellationToken);
            if (company is null)
            {
                return null;
            }
            return new GetCompanySettings()
            {
                CooksNumber = company.CooksNumber,
            };
        }
        public async Task UpdateSettings(Guid companyId, PutCompanySettingsRequest newSettings)
        {
            var company = await this.GetAsync(companyId) ?? throw new EntityNotFoundException(nameof(Company), companyId.ToString());
            company.CooksNumber = newSettings.CooksNumber;
            this.Update(company);
        }
    }
}
