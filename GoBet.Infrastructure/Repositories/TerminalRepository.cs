using GoBet.Application.Interfaces.Repositories;
using GoBet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoBet.Infrastructure.Repositories
{
    public class TerminalRepository(ApplicationDbContext context) : ITerminalRepository
    {

        public async Task<IEnumerable<RoadTerminal>> GetAllAsync()
        {
            return await context.Set<RoadTerminal>().ToListAsync();
        }

        public async Task<IEnumerable<RoadTerminal>> GetByIdsAsync(List<Guid> ids)
        {
            return await context.Set<RoadTerminal>()
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();
        }
    }
}