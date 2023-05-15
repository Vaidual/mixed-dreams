using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.NotFound
{
    public class EntityNotFoundException : NotFoundException
    {
        public EntityNotFoundException(string entityName, string key) : base($"{entityName} with identifier {key} doesn't exist.")
        {
        }
    }
}
