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
        Task AddRequestAsync(PropertyRequest request);
        Task AddReview(Review review);
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task DeleteReview(Review review);
        Task<List<Review>> GetReviews(int id);

        Task<List<Property>> SearchAsync(string state, string status);
        Task AddProperty(Property property);
        Task<List<string>> GetAllFeaturesAsync();
        Task<List<Feature>> GetfeaturesByNamesAsync(List<string> featureNames);
        Task UpdateProperty(Property property);
        Task DeleteProperty(Property property);
        Task<PropertyRequest> GetPropertyRequestByIdAsync(int id);
        Task DeleteRequest(PropertyRequest propertyRequest);
        Task<IEnumerable<Property>> AllPropertiesByAgent(int agentId);
        Task<List<PropertyRequest>> GetPropertyRequestsAsync(int agentid);
        IEnumerable<Property> AllPropertys();
        Task<IEnumerable<Property>> AllProperties();
    }
}