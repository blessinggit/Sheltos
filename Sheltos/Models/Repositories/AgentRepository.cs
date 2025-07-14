using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

namespace Sheltos.Models.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly ApplicationDbContext _context;

        public AgentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            var agents = await _context.Agents
                 .Include(a => a.Properties)
                 .ToListAsync();
            return agents;
        }
        public async Task<Agent> GetAgentByIdAsync(int id)
        {
            var agent = await _context.Agents
                .Include(a => a.Properties)
                .FirstOrDefaultAsync(a => a.AgentId == id);
            return agent;

        }
        public async Task AddAgentAsync(Agent agent)
        {
            var agents = _context.Add(agent);
            await _context.SaveChangesAsync();
        }
    }
}
