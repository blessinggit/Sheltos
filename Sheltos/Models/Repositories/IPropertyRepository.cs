namespace Sheltos.Models.Repositories
{
    public interface IPropertyRepository
    {
        Task<Property> GetByIdAsync(int id);

        Task<IEnumerable<Property>> GetRelatedProperties(string Type);
        Task<IEnumerable<Property>> GetPropertiesByAgentIdAsync(int id);
        Task<IEnumerable<Property>> GetLatestForSaleAsync(int Limit);
        Task<IEnumerable<Property>> GetLatestForRentAsync(int Limit);
        Task AddAsync(Property property);
        bool Save();

        bool Update(Property property);
        Task DeleteAsync(int id);
        IEnumerable<Property> AllPropertys();
        Task<IEnumerable<Property>> AllProperties();

        Task<List<Property>> SearchAsync(string query);
    }
}