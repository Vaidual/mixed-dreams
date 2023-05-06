using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Common
{
    public interface IMustHaveTenant
    {
        public Guid TenantId { get; set; }
    }
}
