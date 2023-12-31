﻿using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery,
            string? sortBy, bool isAscending, int pageNumber = 1, int pageSize = 100);
        Task<Walk> GetByid(Guid id);

        Task<Walk> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk> DeleteByIdAsync(Guid id);
    }
}
