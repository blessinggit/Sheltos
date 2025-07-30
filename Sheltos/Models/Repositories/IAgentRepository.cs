namespace Sheltos.Models.Repositories
{
    public interface IAgentRepository
    {
        Task<IEnumerable<Agent>> GetAllAgentsAsync();
        Task<Agent> GetAgentByIdAsync(int id);
        Task AddAgentAsync(Agent agent);
        Task UpdateAgentAsync(Agent agent);
        Task DeleteAgent(Agent agent);
        Task AddPendingAgent(PendingAgent pendingAgent);
        Task<List<PendingAgent>> PendingAgentsList();
        Task<PendingAgent> GetPendingAgentByIdAsync(int id);
        Task DeletePendingAgent(PendingAgent pendingAgent);
        Task<Agent> GetAgentByEmailAsync(string email);
        Task<PendingAgent> GetPendingAgentByEmailAsync(string email);
        Task<Agent> GetAgentByEmail(string email);

    }
}