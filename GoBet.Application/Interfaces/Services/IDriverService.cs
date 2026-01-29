namespace GoBet.Application.Interfaces.Services
{
    public interface IDriverService
    {
        Task RequestDriverAsync(string PassengerId, string licenseNumber);
    }
}
