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
        public async Task<Agent> GetAgentByEmail(string email)
        {
            return await _context.Agents.Include(a => a.Properties).FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<PendingAgent> GetPendingAgentByIdAsync(int id)
        {
            var pendingAgent = await _context.PendingAgents
                .FirstOrDefaultAsync(a => a.Id == id);
            return pendingAgent;
        }
        public async Task DeletePendingAgent(PendingAgent pendingAgent)
        {
            _context.Remove(pendingAgent);
            await _context.SaveChangesAsync();
        }
        public async Task AddAgentAsync(Agent agent)
        {
            var agents = _context.Add(agent);
            await _context.SaveChangesAsync();
        }
        public async Task<Agent> GetAgentByEmailAsync(string email)
        {
            return await _context.Agents.FirstOrDefaultAsync(a => a.Email == email);
        }
        public async Task AddPendingAgent(PendingAgent pendingAgent)
        {
            _context.Add(pendingAgent);
            await _context.SaveChangesAsync();
        }
        public async Task<PendingAgent> GetPendingAgentByEmailAsync(string email)
        {
            return await _context.PendingAgents.FirstOrDefaultAsync(a => a.Email == email);
        }
        public async Task<List<PendingAgent>> PendingAgentsList()
        {
            return await _context.PendingAgents.ToListAsync();
        }

        public async Task UpdateAgentAsync(Agent agent)
        {
            _context.Update(agent);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAgent(Agent agent)
        {
            _context.Remove(agent);
            await _context.SaveChangesAsync();
        }
        
    }
}
