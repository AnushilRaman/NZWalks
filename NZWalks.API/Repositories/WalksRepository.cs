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


        public async Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery,
            string? sortBy, bool isAscending, int pageNumber, int pageSize)
        {
            var walks = _dbContext.Walks.Include("difficulty").Include("region").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                ////// Filtering of data
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("lengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.LengthInKm == Convert.ToDouble(filterQuery));
                }
                else if (filterOn.Equals("region", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.region.Code == filterQuery);
                }
                else if (filterOn.Equals("difficulty", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.difficulty.Name == filterQuery);
                }
            }
            ///// sorting of data

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);

                }
            }
            ///pagination
            var skipResult = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
        }

        public async Task<Walk> GetByid(Guid id)
        {
            return await _dbContext.Walks.
                Include("difficulty").
                Include("region").
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
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
