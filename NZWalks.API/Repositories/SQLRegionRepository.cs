using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _db;
        public SQLRegionRepository(NZWalksDbContext db)
        {
            _db = db;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _db.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _db.Regions.AddAsync(region);
            await _db.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            Region? existingRagion = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRagion == null)
            {
                return null;
            }

            existingRagion.Code = region.Code;
            existingRagion.Name = region.Name;
            existingRagion.RegionImageUrl = region.RegionImageUrl;

            await _db.SaveChangesAsync();
            return existingRagion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            Region? region = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return null;
            }

            _db.Regions.Remove(region);
            await _db.SaveChangesAsync();
            return region;
        }
    }
}
