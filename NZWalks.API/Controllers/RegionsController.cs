using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper autoMapperProfiles;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository, IMapper autoMapper, ILogger<RegionsController> logger)
        {
            this._regionRepository = regionRepository;
            this.autoMapperProfiles = autoMapper;
            this.logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("Custom exception");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw ex;
            }
            logger.LogInformation("GetAllRegions Action Method Was Invoked");
            logger.LogWarning("Warning");
            logger.LogError("Error");
            var regions = await _regionRepository.GetAllAsync();
            logger.LogInformation($"Fineished GetAllRegions request with data:{JsonSerializer.Serialize(regions)}");

            return Ok(autoMapperProfiles.Map<List<RegionDto>>(regions));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetById(id);
            if (region == null)
                return NotFound();

            var regionDto = autoMapperProfiles.Map<RegionDto>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
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
        [ValidateModel]
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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
