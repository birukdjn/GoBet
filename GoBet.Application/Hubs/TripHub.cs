using Microsoft.AspNetCore.SignalR;
namespace GoBet.Application.Hubs
{
    public class TripHub:Hub
    {
        public async Task JoinTripGroup(string tripId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tripId);
        }
    }
}
