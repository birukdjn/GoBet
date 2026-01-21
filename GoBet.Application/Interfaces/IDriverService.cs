namespace GoBet.Application.Interfaces
{
    public interface IDriverService
    {
        Task RequestDriverAsync(string userId, string licenseNumber);
        Task ApproveDriverAsync(string userId);
    }
}
