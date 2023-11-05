using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery);
        Task<Walk> GetByid(Guid id);

        Task<Walk> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk> DeleteByIdAsync(Guid id);
    }
}
