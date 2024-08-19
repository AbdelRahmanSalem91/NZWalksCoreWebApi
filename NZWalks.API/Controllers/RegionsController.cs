using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private NZWalksDbContext _db;
        public RegionsController(NZWalksDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //Getting data from Domain
            IEnumerable<Region> regionsDomain = _db.Regions.ToList();

            //Mapping data to DTO
            var regionsDto = new List<RegionDto>();

            foreach (Region region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            if (!regionsDto.Any())
            {
                return NotFound("There are no Regions to Show");
            }

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //Getting data from Domain
            Region region = _db.Regions.FirstOrDefault(x => x.Id == id)!;

            //Mapping data to DTO
            RegionDto regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            if (regionDto == null)
            {
                return NotFound($"Region with Code {id} not exist");
            }

            return Ok(regionDto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Mapping DTO to Domain
            Region region = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //Use Domain to Create a Region
            if (region is not null)
            {
                _db.Regions.Add(region);
                _db.SaveChanges();
            }

            //Mapping Domain to DTO
            RegionDto regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, UpdateRegionRequestDto updateRegionRequestDto)
        {
            Region? region = _db.Regions.FirstOrDefault(x => x.Id == id);
            if (region == null)
            {
                return NotFound($"Region with code {id} not exsits");
            }

            //Mapping DTO to Domain
            region.Code = updateRegionRequestDto.Code;
            region.Name = updateRegionRequestDto.Name;
            region.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            _db.SaveChanges();

            //Mapping Domain to DTO
            RegionDto regionDto = new()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            Region? region = _db.Regions.FirstOrDefault(x => x.Id == id);
            if (region == null)
            {
                return NotFound($"Region with code {id} not exsits");
            }

            _db.Regions.Remove(region);
            _db.SaveChanges();

            //Mapping Domain to DTO
            RegionDto regionDto = new()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
