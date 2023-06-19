using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.ServicesInterfaces
{
    public interface ICooksService
    {
        public Task UpdateCooksNumber(Guid companyId, short newCooksNumber);
    }
}
