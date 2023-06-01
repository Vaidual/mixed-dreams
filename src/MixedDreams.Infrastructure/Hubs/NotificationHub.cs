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
            // Broadcast the order notification to all connected chefs
            //await Clients.Group(groupName).SendLowWaterNotification("LowWaterNotification");
            await Clients.All.LowWaterNotification(2);
        }

        public async Task JoinGroup(string TenantId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, TenantId);
        }

    }
}
