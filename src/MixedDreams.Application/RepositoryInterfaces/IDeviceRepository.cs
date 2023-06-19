using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.RepositoryInterfaces
{
    public interface IDeviceRepository : IBaseRepository<Device>
    {
        public Task<string?> GetUserId(string deviceIdentifier, CancellationToken cancellationToken = default);
    }
}
