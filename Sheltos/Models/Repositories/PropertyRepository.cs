

using Microsoft.EntityFrameworkCore;
using Sheltos.Data;

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
                .Include(x => x.Gallery)
                .Include(x => x.Address)
                .Include(x => x.Features)
                .ThenInclude(pf => pf.Feature)
                .OrderByDescending(p => p.DateTime)
                .Take(Limit)
                .ToListAsync();
            return properties;

        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Property>> GetRelatedProperties( string Type)
        {
            var properties = await _context.Properties
                .Where(p => p.Type == Type)
                .Include(x => x.Agent)
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
                 .Include(x => x.Gallery)
                 .Include(x => x.Address)
                 .Include(x => x.Features)
                 .ThenInclude(pf => pf.Feature)
                 .FirstOrDefaultAsync(x => x.Id == id);
            return properties;
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public Task<List<Property>> SearchAsync(string query)
        {
            throw new NotImplementedException();
        }

        public bool Update(Property property)
        {
            throw new NotImplementedException();
        }
    }
}
