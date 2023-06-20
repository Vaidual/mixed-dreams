using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MixedDreams.Application.Repositories
{
    internal class CookRepository : BaseRepository<Cook>, ICookRepository
    {
        public CookRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<Cook>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Table.Include(x => x.CurrentProductPreparation).ToListAsync(cancellationToken);
        }

        public void RemoveRange(IEnumerable<Cook> cooks)
        {
            Table.RemoveRange(cooks);
        }

        public async Task LoadCurrentPreparationsNext(List<Cook> cooks)
        {
            foreach (var cook in cooks)
            {
                if (cook.CurrentProductPreparation != null)
                {
                    await Context.Entry(cook.CurrentProductPreparation)
                        .Reference(p => p.NextProductInQueue)
                        .LoadAsync();
                }
            }
        }
    }
}
