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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerResponseDto>>(customers));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });
            return Ok(_mapper.Map<CustomerResponseDto>(customer));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerRequestDto dto)
        {
            try
            {
                var created = await _customerService.CreateAsync(_mapper.Map<Customer>(dto));
                var response = _mapper.Map<CustomerResponseDto>(created);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
