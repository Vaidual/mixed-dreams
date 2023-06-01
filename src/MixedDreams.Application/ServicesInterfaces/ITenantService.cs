using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Hubs.Clients
{
    public interface ITenantService : ITenantGetter, ITenantSetter { }
    public interface ITenantGetter
    {
        Guid? TenantId { get; }
    }
    public interface ITenantSetter
    {
        void SetTenant(Guid tenantId);
    }
}
