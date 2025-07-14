namespace Sheltos.Models.Repositories
{
    public interface IAgentRepository
    {
        Task<IEnumerable<Agent>> GetAllAgentsAsync();
        Task<Agent> GetAgentByIdAsync(int id);
        Task AddAgentAsync(Agent agent);
    }
}