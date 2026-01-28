namespace GoBet.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task ApproveDriverAsync (string userId);
    }
}
