using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _db;
        public SQLWalkRepository(NZWalksDbContext db)
        {
            _db = db;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await _db.Walks
                .Include("Difficulty")
                .Include("Region")
                .ToListAsync();
        }

        public async Task<Walk?> GetByIDAsync(Guid id)
        {
            return await _db.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _db.Walks.AddAsync(walk);
            await _db.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            Walk? walkModel = await GetByIDAsync(id);

            if(walkModel == null)
            {
                return null;
            }

            walkModel.Name = walk.Name;
            walkModel.Description = walk.Description;
            walkModel.LengthInKm = walk.LengthInKm;
            walkModel.WalkImageUrl = walk.WalkImageUrl;
            walkModel.DifficultyId = walk.DifficultyId;
            walkModel.RegionId = walk.RegionId;

            await _db.SaveChangesAsync();
            return walkModel;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            Walk? walk = await GetByIDAsync(id);

            if (walk == null)
            {
                return null;
            }

            _db.Walks.Remove(walk);
            await _db.SaveChangesAsync();
            return walk;
        }
    }
}
