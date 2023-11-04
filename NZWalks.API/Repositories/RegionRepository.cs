using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbcontext;
        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this._dbcontext = nZWalksDbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbcontext.Regions.AddAsync(region);
            await _dbcontext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbcontext.Regions.ToListAsync();
        }

        public async Task<Region?> GetById([FromRoute] Guid id)
        {
            return await _dbcontext.Regions.FindAsync(id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region updateRegionRequestDto)
        {
            var regionDomainModel = await _dbcontext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (regionDomainModel == null)
            {
                return null;
            }
            //Map Dto to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await _dbcontext.SaveChangesAsync();
            return regionDomainModel;

        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionDomainModel = await _dbcontext.Regions.FindAsync(id);
            if (regionDomainModel == null) { return null; }
            _dbcontext.Regions.Remove(regionDomainModel);
            await _dbcontext.SaveChangesAsync();
            return regionDomainModel;
        }
    }
}
