using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _db;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        public RegionsController(NZWalksDbContext db, IRegionRepository regionRepository, IMapper mapper)
        {
            _db = db;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Getting data from Domain
            List<Region>? regions = await _regionRepository.GetAllAsync();

            if (regions == null)
            {
                return NotFound("There are no Regions to Show");
            }

            //Mapping data to DTO
            return Ok(_mapper.Map<List<RegionDto>>(regions));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //Getting data from Domain
            Region? region = await _regionRepository.GetByIdAsync(id);

            if (region == null)
            {
                return NotFound($"Region with Code {id} not exist");
            }

            //Mapping data to DTO
            return Ok(_mapper.Map<RegionDto>(region));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Mapping DTO to Domain
            Region region = _mapper.Map<Region>(addRegionRequestDto);

            //Use Domain to Create a Region
            if (region is not null)
            {
                region = await _regionRepository.CreateAsync(region);
            }

            //Mapping Domain to DTO
            RegionDto regionDto = _mapper.Map<RegionDto>(region);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Mapping DTO to Domain
            Region? region = _mapper.Map<Region>(updateRegionRequestDto);

            region = await _regionRepository.UpdateAsync(id, region);

            if (region == null)
            {
                return NotFound($"Region with code {id} not exsits");
            }

            //Mapping Domain to DTO
            return Ok(_mapper.Map<RegionDto>(region));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Region? region = await _regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound($"Region with code {id} not exsits");
            }

            //Mapping Domain to DTO
            return Ok(_mapper.Map<RegionDto>(region));
        }
    }
}
