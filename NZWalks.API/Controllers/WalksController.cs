using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _db;
        private readonly IMapper _mapper;
        public WalksController(IWalkRepository db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy = null, [FromQuery] bool? isAscending = true,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            List<Walk> walks = await _db.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            if (walks.Count == 0)
            {
                return NotFound("There is no Walks to show.");
            }

            return Ok(_mapper.Map<List<WalkDto>>(walks));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Walk? walk = await _db.GetByIDAsync(id);

            if (walk == null)
            {
                return NotFound($"Walk with code {id} is not found.");
            }

            return Ok(_mapper.Map<WalkDto>(walk));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domin
            Walk walk = _mapper.Map<Walk>(addWalkRequestDto);

            await _db.CreateAsync(walk);

            // Map Domain to DTO
            return Ok(_mapper.Map<WalkDto>(walk));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map DTO to Domain
            Walk? walk = _mapper.Map<Walk>(updateWalkRequestDto);

            walk = await _db.UpdateAsync(id, walk);

            if (walk == null)
            {
                return NotFound($"Walk with code {id} is not found.");
            }
            //Map Domain to DTO
            return Ok(_mapper.Map<WalkDto>(walk));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Walk? walk = await _db.DeleteAsync(id);

            if (walk == null)
            {
                return NotFound($"Walk with code {id} is not found.");
            }

            //Map Domain to DTO
            return Ok(_mapper.Map<WalkDto>(walk));
        }
    }
}
