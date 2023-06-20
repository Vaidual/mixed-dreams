using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.RepositoryInterfaces
{
    public interface ICookRepository : IBaseRepository<Cook>
    {
        public Task<List<Cook>> GetAllAsync(CancellationToken cancellationToken = default);
        public void RemoveRange(IEnumerable<Cook> cooks);
        public Task LoadCurrentPreparationsNext(List<Cook> cooks);
    }
}
