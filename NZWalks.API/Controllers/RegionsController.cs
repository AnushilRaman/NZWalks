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
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper autoMapperProfiles;

        public RegionsController(IRegionRepository regionRepository, IMapper autoMapper)
        {
            this._regionRepository = regionRepository;
            this.autoMapperProfiles = autoMapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await _regionRepository.GetAllAsync();

            return Ok(autoMapperProfiles.Map<List<RegionDto>>(regions));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetById(id);
            if (region == null)
                return NotFound();

            var regionDto = autoMapperProfiles.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModelAttribute]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert Dto to Domain Model by using automapper

            var regionDomainModel = autoMapperProfiles.Map<Region>(addRegionRequestDto);

            // Use Domain Model to Create Region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model back to Dto by using automapper

            var regionDto = autoMapperProfiles.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map or convert Dto to Domain Model by using automapper
            var regionDomainModel = autoMapperProfiles.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            { return NotFound(); }

            //Map Domain Model back to Dto by using automapper
            var regionDto = autoMapperProfiles.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            { return NotFound(); }

            //Map Domain Model back to Dto by using automapper
            var regionDto = autoMapperProfiles.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
