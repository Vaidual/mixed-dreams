using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Repositories
{
    internal class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(AppDbContext context) : base(context) { }

        public Task<bool> IsNameTaken(string name)
        {
            return ExistAnyAsync(x => x.Name == name);
        }
    }
}
