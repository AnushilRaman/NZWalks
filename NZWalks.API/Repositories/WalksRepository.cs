using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalksRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public WalksRepository(NZWalksDbContext nZWalksDbContext)
        {
            _dbContext = nZWalksDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }

       
        public async Task<List<Walk>> GetAllAsync()
        {
            return await _dbContext.Walks.Include("difficulty").Include("region").ToListAsync();
        }

        public async Task<Walk> GetByid(Guid id)
        {
            return await _dbContext.Walks.
                Include("difficulty").
                Include("region").
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateWalkAsync(Guid id,Walk walk)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.Description = walk.Description;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.RegionId = walk.RegionId;

             await _dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk> DeleteByIdAsync(Guid id)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
             _dbContext.Walks.Remove(existingWalk);
            await _dbContext.SaveChangesAsync();
            return existingWalk;
        }

    }
}
