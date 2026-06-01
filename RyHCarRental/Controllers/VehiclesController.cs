using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RyHCarRental.API.DTOs;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Enums;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<VehicleDto>>(vehicles));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound(new { message = $"Vehículo con ID {id} no encontrado" });
            return Ok(_mapper.Map<VehicleDto>(vehicle));
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleCreateDto dto)
        {
            try
            {
                var created = await _vehicleService.CreateAsync(_mapper.Map<Vehicle>(dto));
                var response = _mapper.Map<VehicleDto>(created);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VehicleUpdateDto dto)
        {
            try
            {
                var vehicle = _mapper.Map<Vehicle>(dto);
                if (!Enum.TryParse<VehicleStatus>(dto.Status, true, out var status))
                    return BadRequest(new { message = $"Estado '{dto.Status}' no es válido." });

                vehicle.Status = status;
                await _vehicleService.UpdateAsync(id, vehicle);
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
                await _vehicleService.DeleteAsync(id);
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
    }
}
