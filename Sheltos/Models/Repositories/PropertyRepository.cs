

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sheltos.Data;
using Sheltos.Data.Enum;

namespace Sheltos.Models.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Property property)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Property> AllPropertys()
        {
            var property = _context.Properties.Include(x => x.Agent)
                .Include(x => x.Gallery)
                .Include(x => x.Address)
                .Include(x => x.Features)
                .ThenInclude(pf => pf.Feature)
                .ToList();
            return property;
        }
        public async Task<IEnumerable<Property>> AllPropertiesByAgent(int agentId)
        {
            var properties = await _context.Properties.Where(x => x.AgentId == agentId)
                .Include(x => x.Agent)
                 .Include(x => x.Gallery)
                 .Include(x => x.Address)
                 .Include(x => x.Features)
                 .ThenInclude(pf => pf.Feature)
                .ToListAsync();
            return properties;
        }

        public async Task<IEnumerable<Property>> AllProperties()
        {
            var properties = await _context.Properties.Include(x => x.Agent)
                 .Include(x => x.Gallery)
                 .Include(x => x.Address)
                 .Include(x => x.Features)
                 .ThenInclude(pf => pf.Feature)
                .ToListAsync();
            return properties;
        }
        public async Task<IEnumerable<Property>> GetPropertiesByAgentIdAsync(int id)
        {
            var properties = await _context.Properties
                .Where(p => p.AgentId == id)
                .Include(x => x.Agent)
                .Include(x => x.Gallery)
                .Include(x => x.Address)
                .Include(x => x.Features)
                .ThenInclude(pf => pf.Feature)
                .ToListAsync();
            return properties;
        }
        public async Task<IEnumerable<Property>> GetLatestForRentAsync(int Limit)
        {
            var properties = await _context.Properties
                .Where(p => p.PropertyStatus == Data.Enum.PropertyStatus.Rent)
                .Include(x => x.Agent)
                .Include(x=>x.Admin)
                .Include(x => x.Gallery)
                .Include(x => x.Address)
                .Include(x => x.Features)
                .ThenInclude(pf => pf.Feature)
                .OrderByDescending(p => p.DateTime)
                .Take(Limit)
                .ToListAsync();
            return properties;

        }
        public async Task<IEnumerable<Property>> GetLatestForSaleAsync(int Limit)
        {
            var properties = await _context.Properties
                .Where(p => p.PropertyStatus == Data.Enum.PropertyStatus.Sale)
                .Include(x => x.Agent)
                .Include(x => x.Admin)
                .Include(x => x.Gallery)
                .Include(x => x.Address)
                .Include(x => x.Features)
                .ThenInclude(pf => pf.Feature)
                .OrderByDescending(p => p.DateTime)
                .Take(Limit)
                .ToListAsync();
            return properties;

        }

        public async Task<List<PropertyRequest>> GetPropertyRequestsAsync()
        {
            return await _context.PropertyRequests
                .Include(pr => pr.Property)
                .ToListAsync();
        }
        public async Task<PropertyRequest> GetPropertyRequestByIdAsync(int id)
        {
            return await _context.PropertyRequests
                .Include(pr => pr.Property)
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }
        public async Task DeleteRequest(PropertyRequest propertyRequest)
        {
            _context.PropertyRequests.Remove(propertyRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Property>> GetRelatedProperties(string Type)
        {
            var properties = await _context.Properties
                .Where(p => p.Type == Type)
                .Include(x => x.Agent)
                .Include(x => x.Admin)
                .Include(x => x.Gallery)
                .Include(x => x.Address)
                .Include(x => x.Features)
                .ThenInclude(pf => pf.Feature)
                .ToListAsync();
            return properties;
        }


        public async Task<Property> GetByIdAsync(int id)
        {
            var properties = await _context.Properties.Include(x => x.Agent)
                    .Include(x => x.Admin)
                 .Include(x => x.Gallery)
                 .Include(x => x.Address)
                 .Include(x => x.Features)
                 .ThenInclude(pf => pf.Feature)
                 .FirstOrDefaultAsync(x => x.Id == id);
            return properties;
        }
        public async Task AddRequestAsync(PropertyRequest request)
        {
            _context.PropertyRequests.Add(request);
            await _context.SaveChangesAsync();


        }
        public async Task AddReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Review>> GetReviews(int id)
        {
            return await _context.Reviews
                .Where(r => r.PropertyId == id)
                .Include(r => r.User)
                .ToListAsync();
        }
        public async Task<Review> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId);
        }
        public async Task DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
        public bool Save()
        {
            throw new NotImplementedException();
        }
        public Task<List<Property>> SearchAsync(string state, string status)
        {
            var query = _context.Properties
                .Include(x => x.Agent)
                .Include(x => x.Admin)
                .Include(x => x.Address)
                .Include(x => x.Gallery)
                .Include(x => x.Features)
                .ThenInclude(pf => pf.Feature)
                .AsQueryable();

            if (!string.IsNullOrEmpty(state))
            {
                query = query.Where(p => p.Address.State.Contains(state));
            }

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<PropertyStatus>(status, out var parsedStatus))
            {
                query = query.Where(p => p.PropertyStatus == parsedStatus);
            }

            return query.ToListAsync();
        }
        public async Task AddProperty(Property property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProperty(Property property)
        {
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProperty(Property property)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
        }
        public async Task<List<string>> GetAllFeaturesAsync()
        {
            return await _context.Features
                .Select(f => f.Name)
                .ToListAsync();
        }
        public async Task<List<Feature>> GetfeaturesByNamesAsync(List<string> featureNames)
        {
            return await _context.Features
                .Where(f => featureNames.Contains(f.Name))
                .ToListAsync();
        }
    }
}
