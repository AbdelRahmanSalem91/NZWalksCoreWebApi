using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _db;
        public SQLWalkRepository(NZWalksDbContext db)
        {
            _db = db;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            [FromQuery] string? sortBy = null, [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Walk> walks = _db.Walks.Include("Difficulty").Include("Region");

            //Filtering
            if (!string.IsNullOrWhiteSpace(filterOn?.Trim()) && !string.IsNullOrWhiteSpace(filterQuery?.Trim()))
            {
                if (filterOn.Trim().Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery.Trim()));
                }
            }

            //Sorting
            if (!string.IsNullOrWhiteSpace(sortBy?.Trim()))
            {
                if (sortBy.Trim().Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Trim().Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            int skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
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
