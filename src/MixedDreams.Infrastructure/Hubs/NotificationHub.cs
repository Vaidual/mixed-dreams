using Microsoft.AspNetCore.SignalR;
using MixedDreams.Infrastructure.Hubs.Clients;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Hubs
{
    public class NotificationHub : Hub<INotifyClient>
    {
        public async Task SendLowWaterNotification(string groupName)
        {
            await Clients.All.LowWaterNotification(2);
        }

        public async Task JoinGroup(string TenantId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, TenantId);
        }

    }
}
