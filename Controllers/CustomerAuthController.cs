using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using Spill_The_Beanz_Coffee_Shop_API.Models;
using Spill_The_Beanz_Coffee_Shop_API.Services;
using System.Security.Cryptography;
using System.Text;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAuthController : ControllerBase
    {
        private readonly CoffeeDbContext _context;
        private readonly JwtService _jwtService;

        public CustomerAuthController(CoffeeDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerRegisterDto dto)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == dto.Email))
                return BadRequest("Email already in use.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var customer = new Customers
            {
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerLoginDto dto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (customer == null || !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
                return Unauthorized("Invalid credentials.");

            customer.LastVisited = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(customer.CustomerID.ToString(), "Customer");
            return Ok(new AuthResponseDto { Token = token });
        }
    }
}
