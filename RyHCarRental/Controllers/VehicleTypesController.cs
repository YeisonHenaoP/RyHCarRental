using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RyHCarRental.API.DTOs.Request;
using RyHCarRental.API.DTOs.Response;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypesController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly IMapper _mapper;

        public VehicleTypesController(IVehicleTypeService vehicleTypeService, IMapper mapper)
        {
            _vehicleTypeService = vehicleTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _vehicleTypeService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<VehicleTypeResponseDto>>(types));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var type = await _vehicleTypeService.GetByIdAsync(id);
            if (type == null)
                return NotFound(new { message = $"Tipo de vehículo con ID {id} no encontrado" });
            return Ok(_mapper.Map<VehicleTypeResponseDto>(type));
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleTypeRequestDto dto)
        {
            try
            {
                var created = await _vehicleTypeService.CreateAsync(_mapper.Map<Domain.Entities.VehicleType>(dto));
                var response = _mapper.Map<VehicleTypeResponseDto>(created);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VehicleTypeRequestDto dto)
        {
            try
            {
                await _vehicleTypeService.UpdateAsync(id, _mapper.Map<Domain.Entities.VehicleType>(dto));
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
                await _vehicleTypeService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
