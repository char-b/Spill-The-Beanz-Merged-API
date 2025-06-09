using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using Spill_The_Beanz_Coffee_Shop_API.Services;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly CoffeeDbContext _context;
        private readonly JwtService _jwtService;

        public AdminAuthController(CoffeeDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto dto)
        {
            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.Email == dto.Email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(dto.Password, admin.Password))
                return Unauthorized("Invalid credentials.");

            admin.lastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(admin.AdminId.ToString(), "Admin");
            return Ok(new AuthResponseDto { Token = token });
        }
    }
}
