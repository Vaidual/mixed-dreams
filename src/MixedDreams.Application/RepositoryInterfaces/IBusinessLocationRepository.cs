using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface IBusinessLocationRepository : IBaseRepository<BusinessLocation>
    {
        public Task<bool> IsNameTaken(string name);
    }
}
