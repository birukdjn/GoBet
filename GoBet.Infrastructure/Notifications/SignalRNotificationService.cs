using GoBet.Application.Hubs;
using GoBet.Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace GoBet.Infrastructure.Notifications
{
    public class SignalRNotificationService(IHubContext<TripHub> hubContext)
        : INotificationService
    {
        public async Task NotifyDriverPickupRequestAsync(Guid tripId, object payload)
        {
            await hubContext.Clients.Group(tripId.ToString())
                .SendAsync("ReceivePickupRequest", payload);
        }
    }
}
