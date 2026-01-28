namespace GoBet.Application.Interfaces.Services
{
    public interface IDriverService
    {
        Task RequestDriverAsync(string userId, string licenseNumber);
    }
}
