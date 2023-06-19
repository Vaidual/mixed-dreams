using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MixedDreams.Application.Features.CompanyFeatures.GetSettings;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Features.CompanyFeatures.PutCompanySettings;

namespace MixedDreams.Application.Repositories
{
    internal class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context) { }

        public async Task<Guid?> GetCompanyIdByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return (await Table.FirstOrDefaultAsync(x => x.ApplicationUserId == userId, cancellationToken))?.Id;
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
                MaxSimultaneousOrders = company.MaxSimultaneousOrders,
                AcceptOrders = company.AcceptOrders,
            };
        }
        public async Task UpdateSettings(Guid companyId, PutCompanySettingsRequest newSettings)
        {
            var company = await this.GetAsync(companyId) ?? throw new EntityNotFoundException(nameof(Company), companyId.ToString());
            company.CooksNumber = newSettings.CooksNumber;
            company.MaxSimultaneousOrders = newSettings.MaxSimultaneousOrders;
            company.AcceptOrders = newSettings.AcceptOrders;
            this.Update(company);
        }
    }
}
