namespace GoBet.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyDriverPickupRequestAsync(Guid tripId, object payload);
    }
}
