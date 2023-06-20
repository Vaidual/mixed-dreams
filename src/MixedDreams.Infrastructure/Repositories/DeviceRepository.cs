using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.Data;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Repositories;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Repositories
{
    internal class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<string?> GetCompanyId(string deviceIdentifier, CancellationToken cancellationToken = default)
        {
            Device device = await Table.FirstOrDefaultAsync(x => x.Identifier == deviceIdentifier, cancellationToken: cancellationToken) ?? 
                throw new EntityNotFoundException(nameof(Device), deviceIdentifier.ToString());
            return device.CompanyId?.ToString();
        }
    }
}
