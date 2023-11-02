using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();

        Task<Region?> GetById([FromRoute] Guid id);

        Task<Region> CreateAsync(Region addRegionRequestDto);

        Task<Region?> UpdateAsync(Guid id, Region addRegionRequestDto);

        Task<Region?> DeleteAsync(Guid id);
    }
}
