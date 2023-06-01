using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface IIngredientRepository : IBaseRepository<Ingredient>
    {
        public Task<bool> IsNameTaken(string name);
    }
}
