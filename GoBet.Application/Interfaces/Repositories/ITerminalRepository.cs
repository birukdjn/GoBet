using GoBet.Domain.Entities;

namespace GoBet.Application.Interfaces.Repositories
{
    public interface ITerminalRepository
    {
        Task<IEnumerable<RoadTerminal>> GetAllAsync();
        Task<IEnumerable<RoadTerminal>> GetByIdsAsync(List<Guid> ids);
    }
}
