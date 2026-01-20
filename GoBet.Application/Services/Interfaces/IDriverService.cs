namespace GoBet.Application.Services.Interfaces
{
    public interface IDriverService
    {
        Task RequestDriverAsync(string userId, string licenseNumber);
        Task ApproveDriverAsync(string userId);
    }
}
