using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RyHCarRental.API.DTOs.Request;
using RyHCarRental.API.DTOs.Response;
using RyHCarRental.Domain.Entities;
using RyHCarRental.Domain.Interfaces.Services;

namespace RyHCarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IMapper _mapper;

        public RentalsController(IRentalService rentalService, IMapper mapper)
        {
            _rentalService = rentalService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rentals = await _rentalService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<RentalResponseDto>>(rentals));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rental = await _rentalService.GetByIdAsync(id);
            if (rental == null)
                return NotFound(new { message = $"Alquiler con ID {id} no encontrado" });
            return Ok(_mapper.Map<RentalResponseDto>(rental));
        }

        [HttpPost]
        public async Task<IActionResult> Create(RentalRequestDto dto)
        {
            try
            {
                var rental = _mapper.Map<Rental>(dto);
                var created = await _rentalService.CreateAsync(rental, dto.VehicleIds);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<RentalResponseDto>(created));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                await _rentalService.CompleteRentalAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _rentalService.CancelRentalAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
