using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _autoMapperProfiles;
        public WalksController(IWalkRepository walkRepository, IMapper autoMapperProfiles)
        {
            _walkRepository = walkRepository;
            _autoMapperProfiles = autoMapperProfiles;
        }
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            var walkDomainmodel = _autoMapperProfiles.Map<Walk>(addWalksRequestDto);
            await _walkRepository.CreateAsync(walkDomainmodel);
            return Ok(_autoMapperProfiles.Map<WalksDto>(walkDomainmodel));

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var walkDomainmodel = await _walkRepository.GetAllAsync(filterOn, filterQuery);
            return Ok(_autoMapperProfiles.Map<List<WalksDto>>(walkDomainmodel));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainmodel = await _walkRepository.GetByid(id);
            if (walkDomainmodel == null)
            {
                return NotFound();
            }

            return Ok(_autoMapperProfiles.Map<WalksDto>(walkDomainmodel));
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalksRequestDto updateWalksRequestDto)
        {
            var walkDomainmodel = _autoMapperProfiles.Map<Walk>(updateWalksRequestDto);
            walkDomainmodel = await _walkRepository.UpdateWalkAsync(id, walkDomainmodel);
            if (walkDomainmodel == null)
                return NotFound();
            return Ok(_autoMapperProfiles.Map<WalksDto>(walkDomainmodel));
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainmodel = await _walkRepository.DeleteByIdAsync(id);
            if (walkDomainmodel == null)
                return NotFound();

            return Ok(_autoMapperProfiles.Map<WalksDto>(walkDomainmodel));
        }
    }
}
