﻿using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.RepositoryInterfaces
{
    public interface IIngredientRepository : IBaseRepository<Ingredient>
    {
        public Task<bool> IsNameTaken(string name);
    }
}
