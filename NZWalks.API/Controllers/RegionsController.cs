using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionsController(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await nZWalksDbContext.regions.ToListAsync();

            var regiondto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regiondto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }

            return Ok(regiondto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = nZWalksDbContext.regions.Find(id);
            var region = await nZWalksDbContext.regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
                return NotFound();

            var regionDto = new RegionDto();
            regionDto.Id = id;
            regionDto.Name = region.Name;
            regionDto.Code = region.Code;
            regionDto.RegionImageUrl = region.RegionImageUrl;
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert Dto to Domain Model

            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };

            // Use Domain Model to Create Region
            await nZWalksDbContext.regions.AddAsync(regionDomainModel);
            nZWalksDbContext.SaveChanges();

            //Map Domain Model back to Dto

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel =await nZWalksDbContext.regions.FindAsync(id);
            if (regionDomainModel == null)
            { return NotFound(); }

            //Map Dto to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await nZWalksDbContext.SaveChangesAsync();
            //Convert Domain to Dto Model
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel =await nZWalksDbContext.regions.FindAsync(id);
            if (regionDomainModel == null)
            { return NotFound(); }
            nZWalksDbContext.regions.Remove(regionDomainModel);
            await nZWalksDbContext.SaveChangesAsync();

            //Convert Domain to Dto Model

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return Ok(regionDto);
        }

    }
}
