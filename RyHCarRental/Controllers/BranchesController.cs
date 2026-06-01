using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RyHCarRental.API.DTOs.Request;
using RyHCarRental.API.DTOs.Response;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public BranchesController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _branchService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BranchResponseDto>>(branches));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _branchService.GetByIdAsync(id);
            if (branch == null)
                return NotFound(new { message = $"Sucursal con ID {id} no encontrada" });
            return Ok(_mapper.Map<BranchResponseDto>(branch));
        }

        [HttpPost]
        public async Task<IActionResult> Create(BranchRequestDto dto)
        {
            try
            {
                var created = await _branchService.CreateAsync(_mapper.Map<Domain.Entities.Branch>(dto));
                var response = _mapper.Map<BranchResponseDto>(created);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BranchRequestDto dto)
        {
            try
            {
                await _branchService.UpdateAsync(id, _mapper.Map<Domain.Entities.Branch>(dto));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _branchService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
