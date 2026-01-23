namespace GoBet.Application.Interfaces
{
    public interface IDriverService
    {
        Task RequestDriverAsync(string userId, string licenseNumber);
        Task<Guid> StartTripAsync(string driverId, string destination, List<Guid> terminalIds);
    }
}
